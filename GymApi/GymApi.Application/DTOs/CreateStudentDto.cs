using GymApi.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace GymApi.Application.DTOs;

public class CreateStudentDto
{
    public string Name { get; set; } = default!;
    public string Email { get; set; } = default!;
    public DateTime BirthDate { get; set; }
    public StudentGoal Goal { get; set; }
}
