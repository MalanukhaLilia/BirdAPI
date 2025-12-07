using BirdAPI_lab4.DTOs;
using BirdAPI_lab4.Repositories;
using FluentValidation;

namespace BirdAPI_lab4.Validators
{
    public class UpdateEggDtoValidator : AbstractValidator<UpdateEggDto>
    {
        private readonly IBirdRepository _birdRepository;

        public UpdateEggDtoValidator(IBirdRepository birdRepository)
        {
            _birdRepository = birdRepository;

            RuleFor(x => x.Size)
                .IsInEnum().WithMessage("Invalid Size. Must be S, M, L, or XL.");

            RuleFor(x => x.BirdId)
                .Cascade(CascadeMode.Stop)
                .GreaterThan(0)
                .MustAsync(BirdExists)
                .WithMessage("Bird with the specified Id does not exist.");
        }

        private async Task<bool> BirdExists(int birdId, CancellationToken token)
        {
            return await _birdRepository.ExistsAsync(birdId);
        }
    }
}