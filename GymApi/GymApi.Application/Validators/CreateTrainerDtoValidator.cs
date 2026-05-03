using FluentValidation;
using GymApi.Application.DTOs;

namespace GymApi.Application.Validators;

public class CreateTrainerDtoValidator : AbstractValidator<CreateTrainerDto>
{
    public CreateTrainerDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(100);
    }
}
