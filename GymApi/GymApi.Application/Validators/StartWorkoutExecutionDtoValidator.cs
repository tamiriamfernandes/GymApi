using FluentValidation;
using GymApi.Application.DTOs;

namespace GymApi.Application.Validators;

public class StartWorkoutExecutionDtoValidator : AbstractValidator<StartWorkoutExecutionDto>
{
    public StartWorkoutExecutionDtoValidator()
    {
        RuleFor(x => x.StudentId)
            .NotEmpty();

        RuleFor(x => x.WorkoutId)
            .NotEmpty();
    }
}
