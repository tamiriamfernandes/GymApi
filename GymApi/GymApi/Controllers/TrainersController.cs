using GymApi.Application.DTOs;
using GymApi.Application.Interfaces;
using GymApi.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace GymApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TrainersController : ControllerBase
    {
        private readonly ITrainerService _trainerService;

        public TrainersController(ITrainerService trainerService)
        {
            _trainerService = trainerService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTrainerDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var id = await _trainerService.CreateAsync(dto);
            return Ok(new { id });
        }

        [HttpGet]
        public async Task<IActionResult> GetPaged(
             [FromQuery] int page = 1,
             [FromQuery] int pageSize = 10)
        {
            var result = await _trainerService.GetPagedAsync(page, pageSize);

            return Ok(new
            {
                data = result.Items,
                total = result.TotalCount,
                page,
                pageSize
            });
        }
    }
}
