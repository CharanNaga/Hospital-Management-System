using Hospital.BedService.Models;
using Hospital.BedService.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.BedService.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class BedsController : ControllerBase
    {
        private readonly IBedRepository _repository;

        public BedsController(IBedRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
            => Ok(await _repository.GetAllAsync());

        [HttpGet("available")]
        public async Task<IActionResult> GetAvailable()
            => Ok(await _repository.GetAvailableAsync());

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var bed = await _repository.GetByIdAsync(id);
            return bed is null ? NotFound() : Ok(bed);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Bed bed)
        {
            await _repository.AddAsync(bed);
            await _repository.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = bed.Id }, bed);
        }

        [HttpPatch("{id:guid}/assign")]
        public async Task<IActionResult> Assign(Guid id, [FromBody] Guid patientId)
        {
            var bed = await _repository.GetByIdAsync(id);
            if (bed is null) return NotFound();
            if (bed.IsOccupied) return Conflict(new { message = "Bed is already occupied" });

            bed.IsOccupied = true;
            bed.PatientId = patientId;
            bed.OccupiedSince = DateTime.UtcNow;
            await _repository.SaveChangesAsync();
            return Ok(bed);
        }

        [HttpPatch("{id:guid}/release")]
        public async Task<IActionResult> Release(Guid id)
        {
            var bed = await _repository.GetByIdAsync(id);
            if (bed is null) return NotFound();

            bed.IsOccupied = false;
            bed.PatientId = null;
            bed.OccupiedSince = null;
            await _repository.SaveChangesAsync();
            return Ok(bed);
        }
    }
}
