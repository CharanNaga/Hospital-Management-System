using Hospital.BedService.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.BedService.Controllers
{
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

        [HttpPost]
        public async Task<IActionResult> Create(Bed bed)
        {
            await _repository.AddAsync(bed);
            await _repository.SaveChangesAsync();
            return Ok(bed);
        }
    }
}
