using GymApi.Domain.Enums;

namespace GymApi.Domain.Entities;

public class ExerciseExecution
{
    public Guid Id { get; private set; }

    public Guid WorkoutExecutionId { get; private set; }
    public WorkoutExecution WorkoutExecution { get; private set; } = default!;

    public Guid ExerciseId { get; private set; }
    public Exercise Exercise { get; private set; } = default!;

    public ExerciseExecutionStatus Status { get; private set; }
    public DateTime? CompletedAt { get; private set; }
    public int? PerformedSets { get; private set; }
    public int? PerformedRepetitions { get; private set; }
    public decimal? PerformedWeight { get; private set; }
    public string? Notes { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    protected ExerciseExecution() { }

    public ExerciseExecution(Guid workoutExecutionId, Guid exerciseId)
    {
        ValidateWorkoutExecutionId(workoutExecutionId);
        ValidateExerciseId(exerciseId);

        Id = Guid.NewGuid();
        WorkoutExecutionId = workoutExecutionId;
        ExerciseId = exerciseId;
        Status = ExerciseExecutionStatus.Pending;
        CreatedAt = DateTime.UtcNow;
    }

    public void Complete()
    {
        Status = ExerciseExecutionStatus.Completed;
        CompletedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Uncomplete()
    {
        Status = ExerciseExecutionStatus.Pending;
        CompletedAt = null;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Skip()
    {
        Status = ExerciseExecutionStatus.Skipped;
        CompletedAt = null;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdatePerformance(int? sets, int? repetitions, decimal? weight, string? notes)
    {
        ValidateSets(sets);
        ValidateRepetitions(repetitions);
        ValidateWeight(weight);

        PerformedSets = sets;
        PerformedRepetitions = repetitions;
        PerformedWeight = weight;
        Notes = notes;
        UpdatedAt = DateTime.UtcNow;
    }

    private static void ValidateWorkoutExecutionId(Guid workoutExecutionId)
    {
        if (workoutExecutionId == Guid.Empty)
        {
            throw new ArgumentException("WorkoutExecutionId is required.");
        }
    }

    private static void ValidateExerciseId(Guid exerciseId)
    {
        if (exerciseId == Guid.Empty)
        {
            throw new ArgumentException("ExerciseId is required.");
        }
    }

    private static void ValidateSets(int? sets)
    {
        if (sets.HasValue && sets.Value <= 0)
        {
            throw new ArgumentException("Sets must be greater than zero when informed.");
        }
    }

    private static void ValidateRepetitions(int? repetitions)
    {
        if (repetitions.HasValue && repetitions.Value <= 0)
        {
            throw new ArgumentException("Repetitions must be greater than zero when informed.");
        }
    }

    private static void ValidateWeight(decimal? weight)
    {
        if (weight is not null && weight < 0)
        {
            throw new ArgumentException("Weight cannot be negative.");
        }
    }
}
