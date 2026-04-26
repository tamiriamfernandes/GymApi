namespace GymApi.Application.DTOs;

public class WorkoutResponseDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public Guid StudentId { get; set; }
    public string StudentName { get; set; }
    public Guid TrainerId { get; set; }
    public string TrainerName { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
