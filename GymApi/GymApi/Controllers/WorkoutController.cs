using GymApi.Application.DTOs;
using GymApi.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GymApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WorkoutController : ControllerBase
{
    private readonly IWorkoutService _workoutService;

    public WorkoutController(IWorkoutService workoutService)
    {
        _workoutService = workoutService;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateWorkoutDto dto)
    {
        var id = await _workoutService.CreateAsync(dto);
        return Created("Created", new { id });
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        var workout = await _workoutService.GetByIdAsync(id);

        return Ok(workout);
    }

    [HttpGet]
    public async Task<IActionResult> GetPaginated([FromQuery] int page, [FromQuery] int pageSize)
    {
        var workout = await _workoutService.GetPagedAsync(page, pageSize);

        return Ok(workout);
    }


    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateWorkoutDto dto)
    {
        await _workoutService.UpdateAsync(id, dto);

        return NoContent();
    }

    [HttpPatch("{id:guid}/activate")]
    public async Task<IActionResult> Activate([FromRoute] Guid id)
    {
        await _workoutService.ActivateAsync(id);

        return NoContent();
    }

    [HttpPatch("{id:guid}/deactivate")]
    public async Task<IActionResult> Deactivate([FromRoute] Guid id)
    {
        await _workoutService.DeactivateAsync(id);

        return NoContent();
    }
}
