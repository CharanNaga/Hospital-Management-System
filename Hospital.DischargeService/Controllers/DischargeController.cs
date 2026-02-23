using Hospital.DischargeService.Repositories;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class DischargeController : ControllerBase
{
    private readonly IAIDietService _ai;

    public DischargeController(IAIDietService ai)
    {
        _ai = ai;
    }

    [HttpPost]
    public IActionResult Create(DischargeSummary summary)
    {
        summary.AIDietRecommendation =
            _ai.GenerateDiet(summary.Diagnosis);

        return Ok(summary);
    }
}