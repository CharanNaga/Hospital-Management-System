using Hospital.BedService.DTOs;
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
        private readonly ILogger<BedsController> _logger;
        public BedsController(
            IBedRepository repository,
            ILogger<BedsController> logger
            )
        {
            _repository = repository;
            _logger = logger;
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
            return bed is null 
                ? NotFound(new { message = $"Bed {id} not found." })
                : Ok(bed);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateBedDto dto)
        {
            var bed = new Bed
            { 
                BedNumber = dto.BedNumber,
                Ward = dto.Ward 
            };

            await _repository.AddAsync(bed);
            await _repository.SaveChangesAsync();

            _logger.LogInformation("Bed {BedId} ({BedNumber}) created in ward {Ward}", bed.Id, bed.BedNumber, bed.Ward);
            return CreatedAtAction(nameof(GetById), new { id = bed.Id }, bed);
        }

        [HttpPatch("{id:guid}/assign")]
        public async Task<IActionResult> Assign(Guid id, [FromBody] AssignBedDto dto)
        {
            var bed = await _repository.GetByIdAsync(id);
            if (bed is null) 
                return NotFound(new 
                { message = $"Bed {id} not found." }
                );

            if (bed.IsOccupied)
                return Conflict(new 
                { message = "Bed is already occupied" }
                );

            bed.IsOccupied = true;
            bed.PatientId = dto.PatientId;
            bed.OccupiedSince = DateTime.UtcNow;
            await _repository.SaveChangesAsync();

            _logger.LogInformation("Bed {BedId} assigned to patient {PatientId}", id, dto.PatientId);
            return Ok(bed);
        }

        [HttpPatch("{id:guid}/release")]
        public async Task<IActionResult> Release(Guid id)
        {
            var bed = await _repository.GetByIdAsync(id);
            if (bed is null) 
                return NotFound(new 
                { message = $"Bed {id} not found." }
                );

            bed.IsOccupied = false;
            bed.PatientId = null;
            bed.OccupiedSince = null;
            await _repository.SaveChangesAsync();
            _logger.LogInformation("Bed {BedId} released (was assigned to patient {PatientId})", id, bed.PatientId);
            return Ok(bed);
        }
    }
}
