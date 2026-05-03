namespace GymApi.Application.DTOs;

public class StartWorkoutExecutionDto
{
    public Guid StudentId { get; set; }
    public Guid WorkoutId { get; set; }
}
