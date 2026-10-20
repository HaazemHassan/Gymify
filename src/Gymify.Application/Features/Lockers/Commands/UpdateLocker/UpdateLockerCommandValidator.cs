using FluentValidation;
using Gymify.Application.Features.Lockers.Shared;

namespace Gymify.Application.Features.Lockers.Commands.UpdateLocker
{
    public class UpdateLockerCommandValidator : AbstractValidator<UpdateLockerCommand>
    {
        public UpdateLockerCommandValidator()
        {
            ApplyValidationRules();
        }

        private void ApplyValidationRules()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("{PropertyName} must be greater than zero");

            RuleFor(x => x.Number)
                .ApplyLockerNumberRules();
        }
    }
}
