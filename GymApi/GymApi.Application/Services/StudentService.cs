using GymApi.Application.DTOs;
using GymApi.Application.Interfaces;
using GymApi.Domain.Entities;

namespace GymApi.Application.Services;

public class StudentService : IStudentService
{
    private readonly IRepository<Student> _repository;

    public StudentService(IRepository<Student> repository)
    {
        _repository = repository;
    }

    public async Task<Guid> CreateAsync(CreateStudentDto dto)
    {
        var student = new Student(
            dto.Name,
            dto.Email,
            DateTime.SpecifyKind(dto.BirthDate, DateTimeKind.Utc),
            dto.Goal
        );

        await _repository.AddAsync(student);

        return student.Id;
    }

    public Task<List<Student>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Student?> GetByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }
}
