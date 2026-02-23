using Hospital.AppointmentService.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.AppointmentService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AppointmentsController : ControllerBase
    {
        private readonly IAppointmentRepository _repository;

        public AppointmentsController(IAppointmentRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
            => Ok(await _repository.GetAllAsync());

        [HttpPost]
        public async Task<IActionResult> Create(Appointment appointment)
        {
            await _repository.AddAsync(appointment);
            await _repository.SaveChangesAsync();
            return Ok(appointment);
        }
    }
}
