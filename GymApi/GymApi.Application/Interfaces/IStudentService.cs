using GymApi.Application.DTOs;
using GymApi.Domain.Entities;

namespace GymApi.Application.Interfaces;

public interface IStudentService
{
    Task<Guid> CreateAsync(CreateStudentDto dto);
    Task<(List<Student> Items, int TotalCount)> GetPagedAsync(int page, int pageSize);
    Task<Student?> GetByIdAsync(Guid id);
}
