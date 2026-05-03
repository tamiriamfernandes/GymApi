using GymApi.Application.DTOs;
using GymApi.Application.Interfaces;
using GymApi.Domain.Entities;
using GymApi.Domain.Enums;

namespace GymApi.Application.Services;

public class WorkoutExecutionService : IWorkoutExecutionService
{
    private readonly IRepository<WorkoutExecution> _workoutExecutionRepository;
    private readonly IRepository<ExerciseExecution> _exerciseExecutionRepository;
    private readonly IRepository<Student> _studentRepository;
    private readonly IRepository<Workout> _workoutRepository;
    private readonly IRepository<Exercise> _exerciseRepository;

    public WorkoutExecutionService(
        IRepository<WorkoutExecution> workoutExecutionRepository,
        IRepository<ExerciseExecution> exerciseExecutionRepository,
        IRepository<Student> studentRepository,
        IRepository<Workout> workoutRepository,
        IRepository<Exercise> exerciseRepository)
    {
        _workoutExecutionRepository = workoutExecutionRepository;
        _exerciseExecutionRepository = exerciseExecutionRepository;
        _studentRepository = studentRepository;
        _workoutRepository = workoutRepository;
        _exerciseRepository = exerciseRepository;
    }

    public async Task<Guid> StartAsync(StartWorkoutExecutionDto dto)
    {
        var student = await _studentRepository.GetByIdAsync(dto.StudentId);
        if (student is null)
        {
            throw new Exception("Student not found");
        }

        var workout = await _workoutRepository.GetByIdAsync(dto.WorkoutId);
        if (workout is null)
        {
            throw new Exception("Workout not found");
        }

        var existingExecutions = await _workoutExecutionRepository.GetAllAsync();
        var inProgressExecution = existingExecutions
            .Where(x => x.StudentId == dto.StudentId && x.WorkoutId == dto.WorkoutId)
            .Where(x => x.Status == WorkoutExecutionStatus.InProgress)
            .OrderByDescending(x => x.CreatedAt)
            .FirstOrDefault();

        if (inProgressExecution is not null)
        {
            return inProgressExecution.Id;
        }

        var exercises = (await _exerciseRepository.GetAllAsync())
            .Where(x => x.WorkoutId == dto.WorkoutId && x.IsActive)
            .OrderBy(x => x.ExecutionOrder)
            .ToList();

        if (exercises.Count == 0)
        {
            throw new Exception("Workout has no exercises");
        }

        var execution = new WorkoutExecution(dto.StudentId, dto.WorkoutId);
        await _workoutExecutionRepository.AddAsync(execution);

        foreach (var exercise in exercises)
        {
            var exerciseExecution = new ExerciseExecution(execution.Id, exercise.Id);
            await _exerciseExecutionRepository.AddAsync(exerciseExecution);
        }

        return execution.Id;
    }

    public async Task<WorkoutExecutionResponseDto?> GetByIdAsync(Guid id)
    {
        var workoutExecution = await _workoutExecutionRepository.GetByIdAsync(id);
        if (workoutExecution is null)
        {
            return null;
        }

        var exerciseExecutions = (await _exerciseExecutionRepository.GetAllAsync())
            .Where(x => x.WorkoutExecutionId == workoutExecution.Id)
            .ToList();

        var exercises = (await _exerciseRepository.GetAllAsync())
            .Where(x => exerciseExecutions.Select(e => e.ExerciseId).Contains(x.Id))
            .ToDictionary(x => x.Id, x => x);

        var exerciseDtos = exerciseExecutions
            .OrderBy(x =>
                exercises.TryGetValue(x.ExerciseId, out var exercise)
                    ? exercise.ExecutionOrder
                    : int.MaxValue)
            .Select(x =>
            {
                exercises.TryGetValue(x.ExerciseId, out var exercise);

                return new ExerciseExecutionResponseDto
                {
                    Id = x.Id,
                    ExerciseId = x.ExerciseId,
                    ExerciseName = exercise?.Name ?? string.Empty,
                    Status = x.Status,
                    CompletedAt = x.CompletedAt,
                    PerformedSets = x.PerformedSets,
                    PerformedRepetitions = x.PerformedRepetitions,
                    PerformedWeight = x.PerformedWeight,
                    Notes = x.Notes
                };
            })
            .ToList();

        return new WorkoutExecutionResponseDto
        {
            Id = workoutExecution.Id,
            StudentId = workoutExecution.StudentId,
            WorkoutId = workoutExecution.WorkoutId,
            Status = workoutExecution.Status,
            StartedAt = workoutExecution.StartedAt,
            FinishedAt = workoutExecution.FinishedAt,
            ExercisesCompleted = exerciseDtos.Count(x => x.Status == ExerciseExecutionStatus.Completed),
            TotalExercises = exerciseDtos.Count,
            Exercises = exerciseDtos
        };
    }

    public async Task CompleteWorkoutAsync(Guid id)
    {
        var workoutExecution = await _workoutExecutionRepository.GetByIdAsync(id);
        if (workoutExecution is null)
        {
            throw new Exception("Workout execution not found");
        }

        workoutExecution.Complete();
        await _workoutExecutionRepository.UpdateAsync(workoutExecution);
    }

    public async Task CancelWorkoutAsync(Guid id)
    {
        var workoutExecution = await _workoutExecutionRepository.GetByIdAsync(id);
        if (workoutExecution is null)
        {
            throw new Exception("Workout execution not found");
        }

        workoutExecution.Cancel();
        await _workoutExecutionRepository.UpdateAsync(workoutExecution);
    }

    public async Task CompleteExerciseAsync(Guid exerciseExecutionId)
    {
        var exerciseExecution = await _exerciseExecutionRepository.GetByIdAsync(exerciseExecutionId);
        if (exerciseExecution is null)
        {
            throw new Exception("Exercise execution not found");
        }

        exerciseExecution.Complete();
        await _exerciseExecutionRepository.UpdateAsync(exerciseExecution);
    }

    public async Task UncompleteExerciseAsync(Guid exerciseExecutionId)
    {
        var exerciseExecution = await _exerciseExecutionRepository.GetByIdAsync(exerciseExecutionId);
        if (exerciseExecution is null)
        {
            throw new Exception("Exercise execution not found");
        }

        exerciseExecution.Uncomplete();
        await _exerciseExecutionRepository.UpdateAsync(exerciseExecution);
    }

    public async Task SkipExerciseAsync(Guid exerciseExecutionId)
    {
        var exerciseExecution = await _exerciseExecutionRepository.GetByIdAsync(exerciseExecutionId);
        if (exerciseExecution is null)
        {
            throw new Exception("Exercise execution not found");
        }

        exerciseExecution.Skip();
        await _exerciseExecutionRepository.UpdateAsync(exerciseExecution);
    }

    public async Task UpdateExercisePerformanceAsync(Guid exerciseExecutionId, UpdateExercisePerformanceDto dto)
    {
        var exerciseExecution = await _exerciseExecutionRepository.GetByIdAsync(exerciseExecutionId);
        if (exerciseExecution is null)
        {
            throw new Exception("Exercise execution not found");
        }

        exerciseExecution.UpdatePerformance(dto.Sets, dto.Repetitions, dto.Weight, dto.Notes);
        await _exerciseExecutionRepository.UpdateAsync(exerciseExecution);
    }
}
