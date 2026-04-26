using GymApi.Application.DTOs;

namespace GymApi.Application.Interfaces;

public interface IWorkoutService
{
    Task<Guid> CreateAsync(CreateWorkoutDto dto);

    Task UpdateAsync(Guid id, UpdateWorkoutDto dto);

    Task<WorkoutResponseDto?> GetByIdAsync(Guid id);

    Task<PagedResult<WorkoutResponseDto>> GetPagedAsync(int page, int pageSize);

    Task ActivateAsync(Guid id);

    Task DeactivateAsync(Guid id);
}
