using FluentValidation;
using Gymify.Application.Features.Lockers.Shared;

namespace Gymify.Application.Features.Lockers.Commands.CreateLocker
{
    public class CreateLockerCommandValidator : AbstractValidator<CreateLockerCommand>
    {
        public CreateLockerCommandValidator()
        {
            ApplyValidationRules();
        }

        private void ApplyValidationRules()
        {
            RuleFor(x => x.Number)
                .ApplyLockerNumberRules();
        }
    }
}
