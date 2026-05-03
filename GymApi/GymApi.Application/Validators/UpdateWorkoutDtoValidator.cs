using FluentValidation;
using GymApi.Application.DTOs;

namespace GymApi.Application.Validators;

public class UpdateWorkoutDtoValidator : AbstractValidator<UpdateWorkoutDto>
{
    public UpdateWorkoutDtoValidator()
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
