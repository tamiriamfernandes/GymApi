using GymApi.Application.DTOs;
using GymApi.Domain.Entities;

namespace GymApi.Application.Interfaces;

public interface IStudentService
{
    Task<Guid> CreateAsync(CreateStudentDto dto);
    Task<List<Student>> GetAllAsync();
    Task<Student?> GetByIdAsync(Guid id);
}
