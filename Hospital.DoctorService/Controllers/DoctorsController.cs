using Hospital.DoctorService.Models;
using Hospital.DoctorService.Repositories;
using Microsoft.AspNetCore.Mvc;

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

    [HttpPost]
    public async Task<IActionResult> Create(Doctor doctor)
    {
        await _repository.AddAsync(doctor);
        await _repository.SaveChangesAsync();
        return Ok(doctor);
    }
}