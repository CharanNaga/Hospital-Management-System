using Hospital.AppointmentService.Models;
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

        public AppointmentsController(IAppointmentRepository repository, ILogger<AppointmentsController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
            => Ok(await _repository.GetAllAsync());

        
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var appt = await _repository.GetByIdAsync(id);
            return appt is null ? NotFound(new { message = "Appointment not found." }) : Ok(appt);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Appointment appointment)
        {
            await _repository.AddAsync(appointment);
            await _repository.SaveChangesAsync();
            _logger.LogInformation("Appointment {Id} created for Patient {PatientId}", appointment.Id, appointment.PatientId);
            return CreatedAtAction(nameof(GetById), new { id = appointment.Id }, appointment);
        }

        [HttpPatch("{id:guid}/status")]
        public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] string status)
        {
            var appt = await _repository.GetByIdAsync(id);
            if (appt is null) return NotFound();
            appt.Status = status;
            await _repository.SaveChangesAsync();
            _logger.LogInformation("Appointment {Id} status changed {Old} → {New}", id, appt.Status, status);
            return Ok(appt);
        }
    }
}
