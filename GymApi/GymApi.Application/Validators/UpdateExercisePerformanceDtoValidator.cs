using FluentValidation;
using GymApi.Application.DTOs;

namespace GymApi.Application.Validators;

public class UpdateExercisePerformanceDtoValidator : AbstractValidator<UpdateExercisePerformanceDto>
{
    public UpdateExercisePerformanceDtoValidator()
    {
        RuleFor(x => x.Sets)
            .GreaterThan(0)
            .When(x => x.Sets.HasValue);

        RuleFor(x => x.Repetitions)
            .GreaterThan(0)
            .When(x => x.Repetitions.HasValue);

        RuleFor(x => x.Weight)
            .GreaterThanOrEqualTo(0)
            .When(x => x.Weight.HasValue);

        RuleFor(x => x.Notes)
            .MaximumLength(1000);
    }
}
