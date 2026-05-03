using GymApi.Domain.Enums;

namespace GymApi.Domain.Entities;

public class Workout
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public Guid StudentId { get; set; }
    public Guid TrainerId { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public Student Student { get; set; }
    public Trainer Trainer { get; set; }
    public ICollection<Exercise> Exercises { get; private set; } = new List<Exercise>();

    public Workout()
    {
        
    }

    public Workout(string name, string description, Guid studentId, Guid trainerId, bool isActive)
    {
        Id = Guid.NewGuid();
        Name = name;
        Description = description;
        StudentId = studentId;
        TrainerId = trainerId;
        IsActive = isActive;
        CreatedAt = DateTime.UtcNow;
    }

    public void UpdateInfo(string name, string description, Guid studentId, Guid trainerId)
    {
        Name = name;
        Description= description;
        StudentId = studentId;
        TrainerId = trainerId;
    }
}
