using Hospital.DoctorService.DTOs;
using Hospital.DoctorService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class DoctorsController : ControllerBase
{
    private readonly IDoctorRepository _repository;
    private readonly ILogger<DoctorsController> _logger;

    public DoctorsController(
        IDoctorRepository repository,
        ILogger<DoctorsController> logger
        )
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
        var doctor = await _repository.GetByIdAsync(id);
        return doctor is null ?
            NotFound(new { message = $"Doctor {id} not found." }) 
            : Ok(doctor);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateDoctorDto dto)
    {
        var doctor = new Doctor
        {
            FullName = dto.FullName,
            Specialization = dto.Specialization,
            Email = dto.Email
        };

        await _repository.AddAsync(doctor);
        await _repository.SaveChangesAsync();
        _logger.LogInformation("Doctor {DoctorId} created: {Name}", doctor.Id, doctor.FullName);
        return CreatedAtAction(
            nameof(GetById), 
            new { id = doctor.Id },
            doctor);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateDoctorDto dto)
    {
        var doctor = await _repository.GetByIdAsync(id);
        if (doctor is null)
            return NotFound(new
            { message = $"Doctor {id} not found." }
            );


        doctor.FullName = dto.FullName;
        doctor.Specialization = dto.Specialization;
        doctor.Email = dto.Email;

        await _repository.SaveChangesAsync();
        _logger.LogInformation("Doctor {DoctorId} updated", id);
        return Ok(doctor);
    }


    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var doctor = await _repository.GetByIdAsync(id);
        if (doctor is null)
            return NotFound(new 
            { message = $"Doctor {id} not found." }
            );

        _repository.Remove(doctor);
        await _repository.SaveChangesAsync();

        _logger.LogInformation("Doctor {DoctorId} deleted by {User}", id, User.Identity?.Name);
        return NoContent();
    }
}