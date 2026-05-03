using GymApi.Application.DTOs;
using GymApi.Application.Interfaces;
using GymApi.Domain.Entities;
using GymApi.Domain.Enums;

namespace GymApi.Application.Services;

public class ProgressService : IProgressService
{
    private readonly IRepository<WorkoutExecution> _workoutExecutionRepository;
    private readonly IRepository<ExerciseExecution> _exerciseExecutionRepository;
    private readonly IRepository<Exercise> _exerciseRepository;
    private readonly IRepository<Student> _studentRepository;
    private readonly IRepository<Workout> _workoutRepository;

    public ProgressService(
        IRepository<WorkoutExecution> workoutExecutionRepository,
        IRepository<ExerciseExecution> exerciseExecutionRepository,
        IRepository<Exercise> exerciseRepository,
        IRepository<Student> studentRepository,
        IRepository<Workout> workoutRepository)
    {
        _workoutExecutionRepository = workoutExecutionRepository;
        _exerciseExecutionRepository = exerciseExecutionRepository;
        _exerciseRepository = exerciseRepository;
        _studentRepository = studentRepository;
        _workoutRepository = workoutRepository;
    }

    public async Task<IEnumerable<WorkoutHistoryResponseDto>> GetStudentWorkoutHistoryAsync(Guid studentId)
    {
        await EnsureStudentExistsAsync(studentId);

        var studentExecutions = (await _workoutExecutionRepository.GetAllAsync())
            .Where(x => x.StudentId == studentId)
            .OrderByDescending(x => x.StartedAt)
            .ToList();

        if (studentExecutions.Count == 0)
        {
            return [];
        }

        var executionIds = studentExecutions
            .Select(x => x.Id)
            .ToHashSet();

        var exerciseExecutions = (await _exerciseExecutionRepository.GetAllAsync())
            .Where(x => executionIds.Contains(x.WorkoutExecutionId))
            .ToList();

        var workoutsById = (await _workoutRepository.GetAllAsync())
            .ToDictionary(x => x.Id, x => x.Name);

        var history = studentExecutions
            .Select(workoutExecution =>
            {
                var executionExercises = exerciseExecutions
                    .Where(x => x.WorkoutExecutionId == workoutExecution.Id)
                    .ToList();

                workoutsById.TryGetValue(workoutExecution.WorkoutId, out var workoutName);

                return new WorkoutHistoryResponseDto
                {
                    WorkoutExecutionId = workoutExecution.Id,
                    WorkoutId = workoutExecution.WorkoutId,
                    WorkoutName = workoutName ?? string.Empty,
                    StartedAt = workoutExecution.StartedAt,
                    FinishedAt = workoutExecution.FinishedAt,
                    DurationInMinutes = CalculateDurationInMinutes(workoutExecution.StartedAt, workoutExecution.FinishedAt),
                    Status = workoutExecution.Status,
                    CompletedExercises = executionExercises.Count(x => x.Status == ExerciseExecutionStatus.Completed),
                    TotalExercises = executionExercises.Count
                };
            })
            .ToList();

        return history;
    }

    public async Task<ExerciseProgressResponseDto?> GetExerciseProgressAsync(Guid studentId, Guid exerciseId)
    {
        await EnsureStudentExistsAsync(studentId);

        var exercise = await _exerciseRepository.GetByIdAsync(exerciseId);
        if (exercise is null)
        {
            throw new Exception("Exercise not found");
        }

        var studentWorkoutExecutions = (await _workoutExecutionRepository.GetAllAsync())
            .Where(x => x.StudentId == studentId)
            .ToList();

        var studentExecutionIds = studentWorkoutExecutions
            .Select(x => x.Id)
            .ToHashSet();

        var history = (await _exerciseExecutionRepository.GetAllAsync())
            .Where(x => studentExecutionIds.Contains(x.WorkoutExecutionId))
            .Where(x => x.ExerciseId == exerciseId)
            .Where(x => x.Status == ExerciseExecutionStatus.Completed)
            .OrderBy(x => x.CompletedAt ?? x.CreatedAt)
            .Select(x => new ExerciseProgressItemDto
            {
                ExecutionDate = x.CompletedAt ?? x.CreatedAt,
                PerformedWeight = x.PerformedWeight,
                PerformedRepetitions = x.PerformedRepetitions,
                PerformedSets = x.PerformedSets
            })
            .ToList();

        return new ExerciseProgressResponseDto
        {
            ExerciseId = exercise.Id,
            ExerciseName = exercise.Name,
            History = history
        };
    }

    public async Task<StudentFrequencyResponseDto> GetWeeklyFrequencyAsync(Guid studentId)
    {
        await EnsureStudentExistsAsync(studentId);

        var (weekStart, weekEndInclusive, weekEndExclusive) = GetCurrentWeekRangeUtc();

        var studentExecutionsInWeek = (await _workoutExecutionRepository.GetAllAsync())
            .Where(x => x.StudentId == studentId)
            .Where(x => x.StartedAt >= weekStart && x.StartedAt < weekEndExclusive)
            .ToList();

        return new StudentFrequencyResponseDto
        {
            StudentId = studentId,
            WeekStart = weekStart,
            WeekEnd = weekEndInclusive,
            TotalWorkouts = studentExecutionsInWeek.Count,
            CompletedWorkouts = studentExecutionsInWeek.Count(x => x.Status == WorkoutExecutionStatus.Completed)
        };
    }

    public async Task<StudentDashboardResponseDto> GetDashboardAsync(Guid studentId)
    {
        await EnsureStudentExistsAsync(studentId);

        var completedExecutions = (await _workoutExecutionRepository.GetAllAsync())
            .Where(x => x.StudentId == studentId)
            .Where(x => x.Status == WorkoutExecutionStatus.Completed)
            .OrderByDescending(x => x.StartedAt)
            .ToList();

        var (weekStart, _, weekEndExclusive) = GetCurrentWeekRangeUtc();

        var durations = completedExecutions
            .Where(x => x.FinishedAt.HasValue)
            .Select(x => (x.FinishedAt!.Value - x.StartedAt).TotalMinutes)
            .ToList();

        var averageDuration = durations.Count == 0
            ? 0
            : Math.Round(durations.Average(), 2);

        var lastWorkoutDate = completedExecutions
            .Select(x => x.FinishedAt ?? x.StartedAt)
            .FirstOrDefault();

        return new StudentDashboardResponseDto
        {
            TotalCompletedWorkouts = completedExecutions.Count,
            CurrentWeekWorkouts = completedExecutions.Count(x => x.StartedAt >= weekStart && x.StartedAt < weekEndExclusive),
            AverageWorkoutDuration = averageDuration,
            LastWorkoutDate = lastWorkoutDate == default ? null : lastWorkoutDate,
            CurrentStreak = CalculateCurrentStreak(completedExecutions)
        };
    }

    private async Task EnsureStudentExistsAsync(Guid studentId)
    {
        var student = await _studentRepository.GetByIdAsync(studentId);
        if (student is null)
        {
            throw new Exception("Student not found");
        }
    }

    private static int? CalculateDurationInMinutes(DateTime startedAt, DateTime? finishedAt)
    {
        if (!finishedAt.HasValue)
        {
            return null;
        }

        var durationInMinutes = (int)Math.Round((finishedAt.Value - startedAt).TotalMinutes);
        return Math.Max(durationInMinutes, 0);
    }

    private static (DateTime WeekStart, DateTime WeekEndInclusive, DateTime WeekEndExclusive) GetCurrentWeekRangeUtc()
    {
        var today = DateTime.UtcNow.Date;
        var diff = (7 + (today.DayOfWeek - DayOfWeek.Monday)) % 7;
        var weekStart = today.AddDays(-diff);
        var weekEndExclusive = weekStart.AddDays(7);
        var weekEndInclusive = weekEndExclusive.AddTicks(-1);

        return (weekStart, weekEndInclusive, weekEndExclusive);
    }

    private static int CalculateCurrentStreak(IEnumerable<WorkoutExecution> completedExecutions)
    {
        var executionDates = completedExecutions
            .Select(x => x.StartedAt.Date)
            .Distinct()
            .OrderByDescending(x => x)
            .ToList();

        if (executionDates.Count == 0)
        {
            return 0;
        }

        var today = DateTime.UtcNow.Date;
        var yesterday = today.AddDays(-1);

        DateTime? currentDate = executionDates.Contains(today)
            ? today
            : executionDates.Contains(yesterday)
                ? yesterday
                : null;

        if (!currentDate.HasValue)
        {
            return 0;
        }

        var streak = 0;
        while (executionDates.Contains(currentDate.Value))
        {
            streak++;
            currentDate = currentDate.Value.AddDays(-1);
        }

        return streak;
    }
}
