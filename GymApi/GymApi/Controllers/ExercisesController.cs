using GymApi.Application.DTOs;
using GymApi.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GymApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ExercisesController : ControllerBase
{
    private readonly IExerciseService _exerciseService;

    public ExercisesController(IExerciseService exerciseService)
    {
        _exerciseService = exerciseService;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateExerciseDto dto)
    {
        try
        {
            var id = await _exerciseService.CreateAsync(dto);
            return Created(nameof(Create), new { id });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex) when (IsNotFoundException(ex))
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateExerciseDto dto)
    {
        try
        {
            await _exerciseService.UpdateAsync(id, dto);
            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex) when (IsNotFoundException(ex))
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        var exercise = await _exerciseService.GetByIdAsync(id);
        if (exercise is null)
        {
            return NotFound();
        }

        return Ok(exercise);
    }

    [HttpGet]
    public async Task<IActionResult> GetPaged([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var paged = await _exerciseService.GetPagedAsync(page, pageSize);
        return Ok(paged);
    }

    [HttpGet("workout/{workoutId:guid}")]
    public async Task<IActionResult> GetByWorkout([FromRoute] Guid workoutId)
    {
        try
        {
            var exercises = await _exerciseService.GetByWorkoutIdAsync(workoutId);
            return Ok(exercises);
        }
        catch (Exception ex) when (IsNotFoundException(ex))
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpPatch("{id:guid}/activate")]
    public async Task<IActionResult> Activate([FromRoute] Guid id)
    {
        try
        {
            await _exerciseService.ActivateAsync(id);
            return NoContent();
        }
        catch (Exception ex) when (IsNotFoundException(ex))
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpPatch("{id:guid}/deactivate")]
    public async Task<IActionResult> Deactivate([FromRoute] Guid id)
    {
        try
        {
            await _exerciseService.DeactivateAsync(id);
            return NoContent();
        }
        catch (Exception ex) when (IsNotFoundException(ex))
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        try
        {
            await _exerciseService.DeleteAsync(id);
            return NoContent();
        }
        catch (Exception ex) when (IsNotFoundException(ex))
        {
            return NotFound(new { message = ex.Message });
        }
    }

    private static bool IsNotFoundException(Exception ex)
    {
        return ex.Message.Contains("not found", StringComparison.OrdinalIgnoreCase);
    }
}
