using GymApi.Domain.Enums;

namespace GymApi.Application.DTOs;

public class ExerciseExecutionResponseDto
{
    public Guid Id { get; set; }
    public Guid ExerciseId { get; set; }
    public string ExerciseName { get; set; } = string.Empty;
    public ExerciseExecutionStatus Status { get; set; }
    public DateTime? CompletedAt { get; set; }
    public int? PerformedSets { get; set; }
    public int? PerformedRepetitions { get; set; }
    public decimal? PerformedWeight { get; set; }
    public string? Notes { get; set; }
}
