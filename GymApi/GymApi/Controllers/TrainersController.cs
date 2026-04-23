using GymApi.Application.DTOs;
using GymApi.Application.Interfaces;
using GymApi.Application.Services;
using GymApi.Domain.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var trainer = await _trainerService.GetByIdAsync(id);

            return Ok(new
            {
                trainer
            });
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateTrainerDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var trainer = await _trainerService.GetByIdAsync(dto.Id);

            if (trainer == null)
                return NotFound();

            trainer = trainer.UpdateInfo(dto.Name, dto.RegistrationNumber);

            var updatedId = await _trainerService.UpdateAsync(trainer);
            return Ok(new { id = updatedId });
        }
    }
}
