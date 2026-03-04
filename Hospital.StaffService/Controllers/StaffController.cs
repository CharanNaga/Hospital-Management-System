using Hospital.StaffService.Models;
using Hospital.StaffService.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.StaffService.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class StaffController : ControllerBase
    {
        private readonly IStaffRepository _repository;
        private readonly ILogger<StaffController> _logger;


        public StaffController(IStaffRepository repository, ILogger<StaffController> logger)
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
            var staff = await _repository.GetByIdAsync(id);
            return staff is null ? NotFound(new { message = $"Staff member {id} not found." }) : Ok(staff);
        }

        [HttpGet("department/{department}")]
        public async Task<IActionResult> GetByDepartment(string department)
            => Ok(await _repository.GetByDepartmentAsync(department));

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] Staff staff)
        {
            await _repository.AddAsync(staff);
            await _repository.SaveChangesAsync();
            _logger.LogInformation("Staff {StaffId} ({Name}) created in {Department}", staff.Id, staff.FullName, staff.Department);
            return CreatedAtAction(nameof(GetById), new { id = staff.Id }, staff);
        }

        [HttpPut("{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(Guid id, [FromBody] Staff updated)
        {
            var staff = await _repository.GetByIdAsync(id);
            if (staff is null) return NotFound();
            staff.FullName = updated.FullName;
            staff.Role = updated.Role;
            staff.Department = updated.Department;
            staff.Email = updated.Email;
            staff.Phone = updated.Phone;
            staff.Shift = updated.Shift;
            await _repository.SaveChangesAsync();
            _logger.LogInformation("Staff {StaffId} updated by {User}", id, User.Identity?.Name);
            return Ok(staff);
        }

        // Soft delete
        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Deactivate(Guid id)
        {
            var staff = await _repository.GetByIdAsync(id);
            if (staff is null || !staff.IsActive) return NotFound(new { message = $"Staff member {id} not found." });
            staff.IsActive = false;
            await _repository.SaveChangesAsync();
            _logger.LogInformation("Staff {StaffId} ({Name}) soft-deleted by {User}", id, staff.FullName, User.Identity?.Name);
            return NoContent();
        }
    }
}
