using FluentValidation;

namespace Gymify.Application.Features.Lockers.Commands.ChangeLockerStatus
{
    public class ChangeLockerStatusCommandValidator : AbstractValidator<ChangeLockerStatusCommand>
    {
        public ChangeLockerStatusCommandValidator()
        {
            ApplyValidationRules();
        }

        private void ApplyValidationRules()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("{PropertyName} must be greater than zero");
        }
    }
}
