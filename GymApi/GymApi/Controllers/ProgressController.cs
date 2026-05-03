using GymApi.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GymApi.Controllers;

[ApiController]
[Route("api/progress")]
public class ProgressController : ControllerBase
{
    private readonly IProgressService _progressService;

    public ProgressController(IProgressService progressService)
    {
        _progressService = progressService;
    }

    [HttpGet("students/{studentId:guid}/history")]
    public async Task<IActionResult> GetStudentWorkoutHistory([FromRoute] Guid studentId)
    {
        try
        {
            var history = await _progressService.GetStudentWorkoutHistoryAsync(studentId);
            return Ok(history);
        }
        catch (Exception ex) when (IsNotFoundException(ex))
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpGet("students/{studentId:guid}/exercises/{exerciseId:guid}")]
    public async Task<IActionResult> GetExerciseProgress(
        [FromRoute] Guid studentId,
        [FromRoute] Guid exerciseId)
    {
        try
        {
            var progress = await _progressService.GetExerciseProgressAsync(studentId, exerciseId);
            return Ok(progress);
        }
        catch (Exception ex) when (IsNotFoundException(ex))
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpGet("students/{studentId:guid}/frequency")]
    public async Task<IActionResult> GetWeeklyFrequency([FromRoute] Guid studentId)
    {
        try
        {
            var frequency = await _progressService.GetWeeklyFrequencyAsync(studentId);
            return Ok(frequency);
        }
        catch (Exception ex) when (IsNotFoundException(ex))
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpGet("students/{studentId:guid}/dashboard")]
    public async Task<IActionResult> GetDashboard([FromRoute] Guid studentId)
    {
        try
        {
            var dashboard = await _progressService.GetDashboardAsync(studentId);
            return Ok(dashboard);
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
