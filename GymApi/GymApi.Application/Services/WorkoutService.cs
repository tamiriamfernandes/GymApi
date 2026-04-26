using GymApi.Application.DTOs;
using GymApi.Application.Interfaces;
using GymApi.Domain.Entities;

namespace GymApi.Application.Services;

public class WorkoutService : IWorkoutService
{
    public readonly IRepository<Workout> _workoutRespository;

    public WorkoutService(IRepository<Workout> workoutRespository)
    {
        _workoutRespository = workoutRespository;
    }

    public async Task ActivateAsync(Guid id)
    {
        var workout = await _workoutRespository.GetByIdAsync(id);
        if(workout is null)
        {
            throw new Exception("Workout not found");
        }

        workout.IsActive = true;
        await _workoutRespository.UpdateAsync(workout);
    }

    public async Task<Guid> CreateAsync(CreateWorkoutDto dto)
    {
        var workout = new Workout(
            dto.Name,
            dto.Description,
            dto.StudentId,
            dto.TrainerId,
            true
        );

        await _workoutRespository.AddAsync(workout);

        return workout.Id;
    }

    public async Task DeactivateAsync(Guid id)
    {
        var workout = await _workoutRespository.GetByIdAsync(id);
        if (workout is null)
        {
            throw new Exception("Workout not found");
        }

        workout.IsActive = false;
        await _workoutRespository.UpdateAsync(workout);
    }

    public async Task<WorkoutResponseDto?> GetByIdAsync(Guid id)
    {
        var workout = await _workoutRespository.GetByIdAsync(id);
        if (workout is null)
        {
            throw new Exception("Workout not found");
        }

        return new WorkoutResponseDto
        {
            Id = workout.Id,
            Name = workout.Name,
            Description = workout.Description,
            StudentId = workout.StudentId,
            TrainerId = workout.TrainerId,
            IsActive = workout.IsActive
        };
    }

    public async Task<PagedResult<WorkoutResponseDto>> GetPagedAsync(int page, int pageSize)
    {
        if (page <= 0) page = 1;
        if (pageSize <= 0) pageSize = 10;

        var pagedResult = await _workoutRespository.GetPagedAsync(page, pageSize);

        return new PagedResult<WorkoutResponseDto> { Data = pagedResult.Items.Select(x => new WorkoutResponseDto()
        {
            Id = x.Id,
            Name = x.Name,
            Description = x.Description,
            StudentId = x.StudentId,
            TrainerId = x.TrainerId,
            IsActive = x.IsActive
        }).ToList(), Page = page, PageSize = pageSize, Total = pagedResult.TotalCount };
    }

    public async Task UpdateAsync(Guid id, UpdateWorkoutDto dto)
    {
        var workout = await _workoutRespository.GetByIdAsync(id);
        if (workout is null)
        {
            throw new Exception("Workout not found");
        }

        workout.UpdateInfo(dto.Name, dto.Description, dto.StudentId, dto.TrainerId);

        await _workoutRespository.UpdateAsync(workout);
    }
}
