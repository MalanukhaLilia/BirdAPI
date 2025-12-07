using BirdAPI_lab4.DTOs;
using FluentValidation;

namespace BirdAPI_lab4.Validators
{
    public class CreateBirdDtoValidator : AbstractValidator<CreateBirdDto>
    {
        public CreateBirdDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name must not be empty.")
                .MaximumLength(50).WithMessage("Name cannot be longer than 50 characters.");

            RuleFor(x => x.Species)
                .NotEmpty().WithMessage("Species must not be empty.")
                .MaximumLength(50).WithMessage("Species cannot be longer than 50 characters.");
        }
    }
}