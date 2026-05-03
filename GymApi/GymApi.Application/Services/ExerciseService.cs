using GymApi.Application.DTOs;
using GymApi.Application.Interfaces;
using GymApi.Domain.Entities;

namespace GymApi.Application.Services;

public class ExerciseService : IExerciseService
{
    private readonly IRepository<Exercise> _exerciseRepository;
    private readonly IRepository<Workout> _workoutRepository;

    public ExerciseService(
        IRepository<Exercise> exerciseRepository,
        IRepository<Workout> workoutRepository)
    {
        _exerciseRepository = exerciseRepository;
        _workoutRepository = workoutRepository;
    }

    public async Task<Guid> CreateAsync(CreateExerciseDto dto)
    {
        var workout = await _workoutRepository.GetByIdAsync(dto.WorkoutId);
        if (workout is null)
        {
            throw new Exception("Workout not found");
        }

        var exercise = new Exercise(
            dto.WorkoutId,
            dto.Name,
            dto.Description,
            dto.Sets,
            dto.Repetitions,
            dto.RestTimeInSeconds,
            dto.Weight,
            dto.ExecutionOrder,
            dto.Notes);

        await _exerciseRepository.AddAsync(exercise);

        return exercise.Id;
    }

    public async Task UpdateAsync(Guid id, UpdateExerciseDto dto)
    {
        var exercise = await _exerciseRepository.GetByIdAsync(id);
        if (exercise is null)
        {
            throw new Exception("Exercise not found");
        }

        exercise.UpdateInfo(
            dto.Name,
            dto.Description,
            dto.Sets,
            dto.Repetitions,
            dto.RestTimeInSeconds,
            dto.Weight,
            dto.Notes,
            dto.ExecutionOrder);

        await _exerciseRepository.UpdateAsync(exercise);
    }

    public async Task<ExerciseResponseDto?> GetByIdAsync(Guid id)
    {
        var exercise = await _exerciseRepository.GetByIdAsync(id);
        if (exercise is null)
        {
            return null;
        }

        var workout = await _workoutRepository.GetByIdAsync(exercise.WorkoutId);
        return ToResponseDto(exercise, workout?.Name);
    }

    public async Task<PagedResult<ExerciseResponseDto>> GetPagedAsync(int page, int pageSize)
    {
        if (page <= 0) page = 1;
        if (pageSize <= 0) pageSize = 10;

        var pagedResult = await _exerciseRepository.GetPagedAsync(page, pageSize);
        var workoutIds = pagedResult.Items.Select(x => x.WorkoutId).Distinct().ToHashSet();
        var workouts = await _workoutRepository.GetAllAsync();
        var workoutNamesById = workouts
            .Where(x => workoutIds.Contains(x.Id))
            .ToDictionary(x => x.Id, x => x.Name);

        return new PagedResult<ExerciseResponseDto>
        {
            Data = pagedResult.Items
                .Select(x => ToResponseDto(
                    x,
                    workoutNamesById.TryGetValue(x.WorkoutId, out var workoutName) ? workoutName : null))
                .ToList(),
            Page = page,
            PageSize = pageSize,
            Total = pagedResult.TotalCount
        };
    }

    public async Task<IEnumerable<ExerciseResponseDto>> GetByWorkoutIdAsync(Guid workoutId)
    {
        var workout = await _workoutRepository.GetByIdAsync(workoutId);
        if (workout is null)
        {
            throw new Exception("Workout not found");
        }

        var exercises = await _exerciseRepository.GetAllAsync();
        return exercises
            .Where(x => x.WorkoutId == workoutId)
            .OrderBy(x => x.ExecutionOrder)
            .Select(x => ToResponseDto(x, workout.Name))
            .ToList();
    }

    public async Task ActivateAsync(Guid id)
    {
        var exercise = await _exerciseRepository.GetByIdAsync(id);
        if (exercise is null)
        {
            throw new Exception("Exercise not found");
        }

        exercise.Activate();
        await _exerciseRepository.UpdateAsync(exercise);
    }

    public async Task DeactivateAsync(Guid id)
    {
        var exercise = await _exerciseRepository.GetByIdAsync(id);
        if (exercise is null)
        {
            throw new Exception("Exercise not found");
        }

        exercise.Deactivate();
        await _exerciseRepository.UpdateAsync(exercise);
    }

    public async Task DeleteAsync(Guid id)
    {
        var exercise = await _exerciseRepository.GetByIdAsync(id);
        if (exercise is null)
        {
            throw new Exception("Exercise not found");
        }

        await _exerciseRepository.DeleteAsync(exercise);
    }

    private static ExerciseResponseDto ToResponseDto(Exercise exercise, string? workoutName)
    {
        return new ExerciseResponseDto
        {
            Id = exercise.Id,
            WorkoutId = exercise.WorkoutId,
            WorkoutName = workoutName ?? string.Empty,
            Name = exercise.Name,
            Description = exercise.Description,
            Sets = exercise.Sets,
            Repetitions = exercise.Repetitions,
            RestTimeInSeconds = exercise.RestTimeInSeconds,
            Weight = exercise.Weight,
            ExecutionOrder = exercise.ExecutionOrder,
            Notes = exercise.Notes,
            IsActive = exercise.IsActive,
            CreatedAt = exercise.CreatedAt,
            UpdatedAt = exercise.UpdatedAt
        };
    }
}
