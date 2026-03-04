using Hospital.DischargeService.Models;
using Hospital.DischargeService.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class DischargeController : ControllerBase
{
    private readonly IDischargeRepository _repository;
    private readonly IAIDietService _ai;
    private readonly ILogger<DischargeController> _logger;


    public DischargeController(IDischargeRepository repository, IAIDietService ai, ILogger<DischargeController> logger)
    {
        _repository = repository;
        _ai = ai;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
        => Ok(await _repository.GetAllAsync());

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var summary = await _repository.GetByIdAsync(id);
        return summary is null ? NotFound(new { message = "Discharge summary not found." }) : Ok(summary);
    }

    [HttpGet("patient/{patientId:guid}")]
    public async Task<IActionResult> GetByPatient(Guid patientId)
        => Ok(await _repository.GetByPatientIdAsync(patientId));

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] DischargeSummary summary)
    {
        summary.AIDietRecommendation = await _ai.GenerateDietAsync(summary.Diagnosis, summary.PatientAge);
        await _repository.AddAsync(summary);
        await _repository.SaveChangesAsync();
        _logger.LogInformation("Discharge created: {Id} for Patient {PatientId}", summary.Id, summary.PatientId);
        return CreatedAtAction(nameof(GetById), new { id = summary.Id }, summary);
    }
}
