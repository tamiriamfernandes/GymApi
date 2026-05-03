using GymApi.Application.DTOs;

namespace GymApi.Application.Interfaces;

public interface IWorkoutExecutionService
{
    Task<Guid> StartAsync(StartWorkoutExecutionDto dto);

    Task<WorkoutExecutionResponseDto?> GetByIdAsync(Guid id);

    Task CompleteWorkoutAsync(Guid id);

    Task CancelWorkoutAsync(Guid id);

    Task CompleteExerciseAsync(Guid exerciseExecutionId);

    Task UncompleteExerciseAsync(Guid exerciseExecutionId);

    Task SkipExerciseAsync(Guid exerciseExecutionId);

    Task UpdateExercisePerformanceAsync(Guid exerciseExecutionId, UpdateExercisePerformanceDto dto);
}
