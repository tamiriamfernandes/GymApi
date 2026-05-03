namespace GymApi.Application.DTOs;

public class UpdateExercisePerformanceDto
{
    public int? Sets { get; set; }
    public int? Repetitions { get; set; }
    public decimal? Weight { get; set; }
    public string? Notes { get; set; }
}
