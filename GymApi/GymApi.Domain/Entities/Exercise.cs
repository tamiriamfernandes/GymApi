namespace GymApi.Domain.Entities;

public class Exercise
{
    public Guid Id { get; private set; }

    public Guid WorkoutId { get; private set; }
    public Workout Workout { get; private set; } = default!;

    public string Name { get; private set; } = default!;
    public string? Description { get; private set; }
    public int Sets { get; private set; }
    public int Repetitions { get; private set; }
    public int? RestTimeInSeconds { get; private set; }
    public decimal? Weight { get; private set; }
    public int ExecutionOrder { get; private set; }
    public string? Notes { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    protected Exercise() { }

    public Exercise(
        Guid workoutId,
        string name,
        string? description,
        int sets,
        int repetitions,
        int? restTimeInSeconds,
        decimal? weight,
        int executionOrder,
        string? notes)
    {
        ValidateWorkoutId(workoutId);
        ValidateName(name);
        ValidateSets(sets);
        ValidateRepetitions(repetitions);
        ValidateExecutionOrder(executionOrder);
        ValidateWeight(weight);

        Id = Guid.NewGuid();
        WorkoutId = workoutId;
        Name = name.Trim();
        Description = description;
        Sets = sets;
        Repetitions = repetitions;
        RestTimeInSeconds = restTimeInSeconds;
        Weight = weight;
        ExecutionOrder = executionOrder;
        Notes = notes;
        IsActive = true;
        CreatedAt = DateTime.UtcNow;
    }

    public void UpdateInfo(
        string name,
        string? description,
        int sets,
        int repetitions,
        int? restTimeInSeconds,
        decimal? weight,
        string? notes,
        int executionOrder)
    {
        ValidateName(name);
        ValidateSets(sets);
        ValidateRepetitions(repetitions);
        ValidateExecutionOrder(executionOrder);
        ValidateWeight(weight);

        Name = name.Trim();
        Description = description;
        Sets = sets;
        Repetitions = repetitions;
        RestTimeInSeconds = restTimeInSeconds;
        Weight = weight;
        Notes = notes;
        ExecutionOrder = executionOrder;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Activate()
    {
        IsActive = true;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        IsActive = false;
        UpdatedAt = DateTime.UtcNow;
    }

    public void ChangeOrder(int order)
    {
        ValidateExecutionOrder(order);
        ExecutionOrder = order;
        UpdatedAt = DateTime.UtcNow;
    }

    private static void ValidateWorkoutId(Guid workoutId)
    {
        if (workoutId == Guid.Empty)
        {
            throw new ArgumentException("WorkoutId is required.");
        }
    }

    private static void ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Name is required.");
        }
    }

    private static void ValidateSets(int sets)
    {
        if (sets <= 0)
        {
            throw new ArgumentException("Sets must be greater than zero.");
        }
    }

    private static void ValidateRepetitions(int repetitions)
    {
        if (repetitions <= 0)
        {
            throw new ArgumentException("Repetitions must be greater than zero.");
        }
    }

    private static void ValidateExecutionOrder(int executionOrder)
    {
        if (executionOrder < 0)
        {
            throw new ArgumentException("ExecutionOrder must be greater than or equal to zero.");
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
