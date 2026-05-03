using GymApi.Domain.Enums;

namespace GymApi.Domain.Entities;

public class WorkoutExecution
{
    public Guid Id { get; private set; }

    public Guid StudentId { get; private set; }
    public Student Student { get; private set; } = default!;

    public Guid WorkoutId { get; private set; }
    public Workout Workout { get; private set; } = default!;

    public WorkoutExecutionStatus Status { get; private set; }
    public DateTime StartedAt { get; private set; }
    public DateTime? FinishedAt { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    public ICollection<ExerciseExecution> ExerciseExecutions { get; private set; } = new List<ExerciseExecution>();

    protected WorkoutExecution() { }

    public WorkoutExecution(Guid studentId, Guid workoutId)
    {
        ValidateStudentId(studentId);
        ValidateWorkoutId(workoutId);

        Id = Guid.NewGuid();
        StudentId = studentId;
        WorkoutId = workoutId;
        Status = WorkoutExecutionStatus.InProgress;
        StartedAt = DateTime.UtcNow;
        CreatedAt = DateTime.UtcNow;
    }

    public void Complete()
    {
        if (Status == WorkoutExecutionStatus.Cancelled)
        {
            throw new InvalidOperationException("Cannot complete a cancelled workout execution.");
        }

        Status = WorkoutExecutionStatus.Completed;
        FinishedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Cancel()
    {
        if (Status == WorkoutExecutionStatus.Completed)
        {
            throw new InvalidOperationException("Cannot cancel a completed workout execution.");
        }

        Status = WorkoutExecutionStatus.Cancelled;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateStatus(WorkoutExecutionStatus status)
    {
        if (status == WorkoutExecutionStatus.Completed)
        {
            Complete();
            return;
        }

        if (status == WorkoutExecutionStatus.Cancelled)
        {
            Cancel();
            return;
        }

        Status = status;
        UpdatedAt = DateTime.UtcNow;
    }

    private static void ValidateStudentId(Guid studentId)
    {
        if (studentId == Guid.Empty)
        {
            throw new ArgumentException("StudentId is required.");
        }
    }

    private static void ValidateWorkoutId(Guid workoutId)
    {
        if (workoutId == Guid.Empty)
        {
            throw new ArgumentException("WorkoutId is required.");
        }
    }
}
