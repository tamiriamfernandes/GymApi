namespace GymApi.Application.DTOs;

public class ExerciseProgressResponseDto
{
    public Guid ExerciseId { get; set; }
    public string ExerciseName { get; set; } = string.Empty;
    public List<ExerciseProgressItemDto> History { get; set; } = new();
}
