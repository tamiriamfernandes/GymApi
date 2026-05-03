using FluentValidation;
using GymApi.Application.DTOs;

namespace GymApi.Application.Validators;

public class UpdateTrainerDtoValidator : AbstractValidator<UpdateTrainerDto>
{
    public UpdateTrainerDtoValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(100);
    }
}
