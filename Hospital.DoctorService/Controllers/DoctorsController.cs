using Hospital.DoctorService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class DoctorsController : ControllerBase
{
    private readonly IDoctorRepository _repository;

    public DoctorsController(IDoctorRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
        => Ok(await _repository.GetAllAsync());

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var doctor = await _repository.GetByIdAsync(id);
        return doctor is null ? NotFound() : Ok(doctor);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Doctor doctor)
    {
        await _repository.AddAsync(doctor);
        await _repository.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = doctor.Id }, doctor);
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var doctor = await _repository.GetByIdAsync(id);
        if (doctor is null) return NotFound();
        _repository.Remove(doctor);
        await _repository.SaveChangesAsync();
        return NoContent();
    }
}
