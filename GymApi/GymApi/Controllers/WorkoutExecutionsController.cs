using GymApi.Application.DTOs;
using GymApi.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GymApi.Controllers;

[ApiController]
[Route("api/workout-executions")]
public class WorkoutExecutionsController : ControllerBase
{
    private readonly IWorkoutExecutionService _workoutExecutionService;

    public WorkoutExecutionsController(IWorkoutExecutionService workoutExecutionService)
    {
        _workoutExecutionService = workoutExecutionService;
    }

    [HttpPost("start")]
    public async Task<IActionResult> Start([FromBody] StartWorkoutExecutionDto dto)
    {
        try
        {
            var id = await _workoutExecutionService.StartAsync(dto);
            return Ok(new { id });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
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
        var execution = await _workoutExecutionService.GetByIdAsync(id);
        if (execution is null)
        {
            return NotFound();
        }

        return Ok(execution);
    }

    [HttpPatch("{id:guid}/complete")]
    public async Task<IActionResult> CompleteWorkout([FromRoute] Guid id)
    {
        try
        {
            await _workoutExecutionService.CompleteWorkoutAsync(id);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex) when (IsNotFoundException(ex))
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpPatch("{id:guid}/cancel")]
    public async Task<IActionResult> CancelWorkout([FromRoute] Guid id)
    {
        try
        {
            await _workoutExecutionService.CancelWorkoutAsync(id);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex) when (IsNotFoundException(ex))
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpPatch("/api/exercise-executions/{id:guid}/complete")]
    public async Task<IActionResult> CompleteExercise([FromRoute] Guid id)
    {
        return await ExecuteExerciseActionAsync(id, _workoutExecutionService.CompleteExerciseAsync);
    }

    [HttpPatch("/api/exercise-executions/{id:guid}/uncomplete")]
    public async Task<IActionResult> UncompleteExercise([FromRoute] Guid id)
    {
        return await ExecuteExerciseActionAsync(id, _workoutExecutionService.UncompleteExerciseAsync);
    }

    [HttpPatch("/api/exercise-executions/{id:guid}/skip")]
    public async Task<IActionResult> SkipExercise([FromRoute] Guid id)
    {
        return await ExecuteExerciseActionAsync(id, _workoutExecutionService.SkipExerciseAsync);
    }

    [HttpPatch("/api/exercise-executions/{id:guid}/performance")]
    public async Task<IActionResult> UpdateExercisePerformance(
        [FromRoute] Guid id,
        [FromBody] UpdateExercisePerformanceDto dto)
    {
        try
        {
            await _workoutExecutionService.UpdateExercisePerformanceAsync(id, dto);
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

    private async Task<IActionResult> ExecuteExerciseActionAsync(
        Guid exerciseExecutionId,
        Func<Guid, Task> action)
    {
        try
        {
            await action(exerciseExecutionId);
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
