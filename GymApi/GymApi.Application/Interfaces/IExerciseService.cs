using GymApi.Application.DTOs;

namespace GymApi.Application.Interfaces;

public interface IExerciseService
{
    Task<Guid> CreateAsync(CreateExerciseDto dto);

    Task UpdateAsync(Guid id, UpdateExerciseDto dto);

    Task<ExerciseResponseDto?> GetByIdAsync(Guid id);

    Task<PagedResult<ExerciseResponseDto>> GetPagedAsync(int page, int pageSize);

    Task<IEnumerable<ExerciseResponseDto>> GetByWorkoutIdAsync(Guid workoutId);

    Task ActivateAsync(Guid id);

    Task DeactivateAsync(Guid id);

    Task DeleteAsync(Guid id);
}
