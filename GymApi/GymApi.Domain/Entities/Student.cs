using GymApi.Domain.Enums;

namespace GymApi.Domain.Entities;

public class Student
{
    public Guid Id { get; private set; }

    public string Name { get; private set; } = default!;
    public string Email { get; private set; } = default!;
    public DateTime BirthDate { get; private set; }

    public StudentGoal Goal { get; private set; }

    // optional link to system user
    public Guid? UserId { get; private set; }
    // public User? User { get; private set; }

    // navigation
    //public ICollection<Workout> Workouts { get; private set; } = new List<Workout>();

    // EF Core
    protected Student() { }

    public Student(string name, string email, DateTime birthDate, StudentGoal goal)
    {
        Id = Guid.NewGuid();
        Name = name;
        Email = email;
        BirthDate = birthDate;
        Goal = goal;
    }

    public void UpdateInfo(string name, string email, DateTime birthDate)
    {
        Name = name;
        Email = email;
        BirthDate = birthDate;
    }

    public void SetGoal(StudentGoal goal)
    {
        Goal = goal;
    }

    public void LinkUser(Guid userId)
    {
        UserId = userId;
    }
}
