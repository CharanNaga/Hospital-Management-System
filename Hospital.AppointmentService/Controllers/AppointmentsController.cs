using Hospital.AppointmentService.DTOs;
using Hospital.AppointmentService.Models;
using Hospital.AppointmentService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.AppointmentService.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AppointmentsController : ControllerBase
    {
        private readonly IAppointmentRepository _repository;
        private readonly ILogger<AppointmentsController> _logger;
        private readonly IEmailService _emailService;

        public AppointmentsController(
            IAppointmentRepository repository, 
            ILogger<AppointmentsController> logger,
            IEmailService emailService
            )
        {
            _repository = repository;
            _logger = logger;
            _emailService = emailService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
            => Ok(await _repository.GetAllAsync());

        
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var appt = await _repository.GetByIdAsync(id);
            return appt is null
                ? NotFound(new { message = "Appointment not found." })
                : Ok(appt);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateAppointmentDto dto)
        {
            if (await _repository.HasConflictAsync(dto.DoctorId, dto.AppointmentDate))
            {
                _logger.LogWarning("Booking conflict for DoctorId={DoctorId} at {Date}", dto.DoctorId, dto.AppointmentDate);
                return Conflict(new { message = "Doctor already has an appointment within 30 minutes of this time slot." });
            }

            var appointment = new Appointment
            {
                PatientId = dto.PatientId,
                DoctorId = dto.DoctorId,
                AppointmentDate = dto.AppointmentDate,
                Notes = dto.Notes,
                PatientName = dto.PatientName,
                DoctorName = dto.DoctorName
            };

            // Send email — fire-and-forget with error handling (don't fail the request)
            _ = Task.Run(async () =>
            {
                try
                {
                    await _emailService.SendAppointmentConfirmationAsync(
                        dto.PatientEmail, 
                        dto.PatientName ?? "Patient",
                        dto.DoctorName ?? "Doctor",
                        dto.DoctorEmail,
                        dto.AppointmentDate,
                        dto.Notes ?? string.Empty);
                }
                catch (Exception ex)
                { _logger.LogError(ex, "Failed to send appointment email"); }
            });



            await _repository.AddAsync(appointment);
            await _repository.SaveChangesAsync();

            _logger.LogInformation("Appointment {Id} created for Patient {PatientId}", appointment.Id, appointment.PatientId);
            return CreatedAtAction(nameof(GetById), new { id = appointment.Id }, appointment);
        }

        [HttpPatch("{id:guid}/status")]
        public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] UpdateAppointmentStatusDto dto)
        {
            var appt = await _repository.GetByIdAsync(id);
            if (appt is null) 
                return NotFound(new
                { message = "Appointment not found." }
                );
            // cannot change a Completed/Cancelled appointment
            if (appt.Status is "Completed" or "Cancelled")
                return UnprocessableEntity(new { message = $"Cannot change status of a {appt.Status} appointment." });

            var oldStatus = appt.Status;
            appt.Status = dto.Status;
            await _repository.SaveChangesAsync();

            _logger.LogInformation("Appointment {Id} status changed {Old} → {New}", id, oldStatus, dto.Status);
            return Ok(appt);
        }
    }
}
