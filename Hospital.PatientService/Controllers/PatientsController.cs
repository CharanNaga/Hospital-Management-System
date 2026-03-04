using Hospital.PatientService.DTOs;
using Hospital.PatientService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class PatientsController : ControllerBase
{
    private readonly IPatientRepository _repository;
    private readonly ILogger<PatientsController> _logger;

    public PatientsController(
        IPatientRepository repository,
        ILogger<PatientsController> logger
        )
    {
        _repository = repository;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
        => Ok(await _repository.GetAllAsync());

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var patient = await _repository.GetByIdAsync(id);
        return patient is null
            ? NotFound(new { message = $"Patient {id} not found." })
            : Ok(patient);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreatePatientDto dto)
    {
        var patient = new Patient
        {
            FullName = dto.FullName,
            Age = dto.Age,
            Gender = dto.Gender,
            Phone = dto.Phone,
            Email = dto.Email,
            Address = dto.Address
        };

        await _repository.AddAsync(patient);
        await _repository.SaveChangesAsync();

        _logger.LogInformation("Patient {PatientId} created: {Name}", patient.Id, patient.FullName);

        return CreatedAtAction(nameof(GetById), 
            new { id = patient.Id },
            patient);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdatePatientDto dto)
    {
        var patient = await _repository.GetByIdAsync(id);
        if (patient is null)
            return NotFound(new
            { message = $"Patient {id} not found." });

        patient.FullName = dto.FullName;
        patient.Age = dto.Age;
        patient.Gender = dto.Gender;
        patient.Phone = dto.Phone;
        patient.Email = dto.Email;
        patient.Address = dto.Address;

        await _repository.SaveChangesAsync();
        _logger.LogInformation("Patient {PatientId} updated", id);
        return Ok(patient);
    }


    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var patient = await _repository.GetByIdAsync(id);
        if (patient is null) return NotFound(new { message = $"Patient {id} not found." });

        _repository.Remove(patient);
        await _repository.SaveChangesAsync();

        _logger.LogInformation("Patient {PatientId} deleted by {User}", id, User.Identity?.Name);
        return NoContent();
    }
}
