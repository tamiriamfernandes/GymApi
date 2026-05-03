namespace GymApi.Application.DTOs;

public class ExerciseResponseDto
{
    public Guid Id { get; set; }
    public Guid WorkoutId { get; set; }
    public string WorkoutName { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int Sets { get; set; }
    public int Repetitions { get; set; }
    public int? RestTimeInSeconds { get; set; }
    public decimal? Weight { get; set; }
    public int ExecutionOrder { get; set; }
    public string? Notes { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
