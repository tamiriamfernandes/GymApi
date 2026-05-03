using GymApi.Domain.Enums;

namespace GymApi.Application.DTOs;

public class WorkoutExecutionResponseDto
{
    public Guid Id { get; set; }
    public Guid StudentId { get; set; }
    public Guid WorkoutId { get; set; }
    public WorkoutExecutionStatus Status { get; set; }
    public DateTime StartedAt { get; set; }
    public DateTime? FinishedAt { get; set; }
    public int ExercisesCompleted { get; set; }
    public int TotalExercises { get; set; }
    public List<ExerciseExecutionResponseDto> Exercises { get; set; } = new();
}
