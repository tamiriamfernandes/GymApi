using FluentValidation;
using GymApi.Application.DTOs;

namespace GymApi.Application.Validators;

public class UpdateExerciseDtoValidator : AbstractValidator<UpdateExerciseDto>
{
    public UpdateExerciseDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(150);

        RuleFor(x => x.Description)
            .MaximumLength(500);

        RuleFor(x => x.Sets)
            .GreaterThan(0);

        RuleFor(x => x.Repetitions)
            .GreaterThan(0);

        RuleFor(x => x.ExecutionOrder)
            .GreaterThanOrEqualTo(0);

        RuleFor(x => x.Weight)
            .GreaterThanOrEqualTo(0)
            .When(x => x.Weight.HasValue);

        RuleFor(x => x.Notes)
            .MaximumLength(1000);
    }
}
