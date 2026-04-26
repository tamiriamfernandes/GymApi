namespace GymApi.Application.DTOs;

public class CreateWorkoutDto
{
    public string Name { get; set; }
    public string Description { get; set; }
    public Guid StudentId { get; set; }
    public Guid TrainerId { get; set; }
}
