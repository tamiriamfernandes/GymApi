using FluentValidation;
using GymApi.Application.DTOs;

namespace GymApi.Application.Validators;

public class CreateWorkoutDtoValidator : AbstractValidator<CreateWorkoutDto>
{
    public CreateWorkoutDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(150);

        RuleFor(x => x.Description)
            .NotEmpty()
            .MaximumLength(500);

        RuleFor(x => x.StudentId)
            .NotEmpty();

        RuleFor(x => x.TrainerId)
            .NotEmpty();
    }
}
