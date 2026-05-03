using GymApi.Domain.Enums;

namespace GymApi.Application.DTOs;

public class WorkoutHistoryResponseDto
{
    public Guid WorkoutExecutionId { get; set; }
    public Guid WorkoutId { get; set; }
    public string WorkoutName { get; set; } = string.Empty;
    public DateTime StartedAt { get; set; }
    public DateTime? FinishedAt { get; set; }
    public int? DurationInMinutes { get; set; }
    public WorkoutExecutionStatus Status { get; set; }
    public int CompletedExercises { get; set; }
    public int TotalExercises { get; set; }
}
