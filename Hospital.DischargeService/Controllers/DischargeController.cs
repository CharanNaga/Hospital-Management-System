using Hospital.DischargeService.DTOs;
using Hospital.DischargeService.Models;
using Hospital.DischargeService.Repositories;
using Hospital.DischargeService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class DischargeController : ControllerBase
{
    private readonly IDischargeRepository _repository;
    private readonly IAIDietService _ai;
    private readonly IQuestPdfReportService _pdfReportService;
    private readonly ILogger<DischargeController> _logger;
    private readonly IPatientLookupService _patientLookup;


    public DischargeController(
        IDischargeRepository repository,
        IAIDietService ai,
        IQuestPdfReportService pdfReportService,
        ILogger<DischargeController> logger,
        IPatientLookupService patientLookup
        )
    {
        _repository = repository;
        _ai = ai;
        _pdfReportService = pdfReportService;
        _logger = logger;
        _patientLookup = patientLookup;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
        => Ok(await _repository.GetAllAsync());

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var summary = await _repository.GetByIdAsync(id);
        return summary is null 
            ? NotFound(new { message = "Discharge summary not found." })
            : Ok(summary);
    }

    [HttpGet("patient/{patientId:guid}")]
    public async Task<IActionResult> GetByPatient(Guid patientId)
        => Ok(await _repository.GetByPatientIdAsync(patientId));

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] DischargeSummaryDto dto)
    {
        //    This gives us FullName, Age, Gender — the caller does NOT supply these
        var patient = await _patientLookup.GetPatientAsync(dto.PatientId);
        if (patient is null)
        {
            return NotFound(new
            {
                message = $"Patient {dto.PatientId} was not found in PatientService. " +
                          "Ensure the patient exists before creating a discharge record."
            });
        }


        var summary = new DischargeSummary
        {
            PatientId = dto.PatientId,
            //PatientName = dto.PatientName,
            //PatientAge = dto.PatientAge,
            //PatientGender = dto.PatientGender,
            PatientName = patient.FullName,
            PatientAge = patient.Age,
            PatientGender = patient.Gender,
            AdmittedOn = dto.AdmittedOn.HasValue ? dto.AdmittedOn.Value : DateTime.UtcNow,
            Diagnosis = dto.Diagnosis,
            Treatment = dto.Treatment,
            Medications = dto.Medications,
            FollowUpInstructions = dto.FollowUpInstructions,
            DischargingDoctorId = dto.DischargingDoctorId
        };

        summary.AIDietRecommendation = await _ai.GenerateDietAsync(summary.Diagnosis, summary.PatientAge);

        await _repository.AddAsync(summary);
        await _repository.SaveChangesAsync();

        _logger.LogInformation("Discharge created: {Id} for Patient {PatientId}", summary.Id, summary.PatientId);
        return CreatedAtAction(nameof(GetById), new { id = summary.Id }, summary);
    }

    // ── GET PDF download ─────────────────────────────────────────────────────
    [HttpGet("{id:guid}/pdf")]
    public async Task<IActionResult> DownloadPdf(Guid id)
    {
        var summary = await _repository.GetByIdAsync(id);
        if (summary is null) 
            return NotFound(new
            { message = "Discharge summary not found." }
            );

        var pdfBytes = _pdfReportService.GenerateDischargePdf(summary);
        var fileName = $"DischargeSummary_{summary.PatientName?.Replace(" ", "_")}_{summary.DischargedOn:yyyyMMdd}.pdf";

        _logger.LogInformation("PDF downloaded for discharge {Id}", id);
        return File(pdfBytes, "application/pdf", fileName);
    }
}
