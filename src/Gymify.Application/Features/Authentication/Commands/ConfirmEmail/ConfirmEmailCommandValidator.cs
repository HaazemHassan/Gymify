using FluentValidation;
using Gymify.Application.Common;

namespace Gymify.Application.Features.Authentication.Commands.ConfirmEmail
{
    public class ConfirmEmailCommandValidator : AbstractValidator<ConfirmEmailCommand>
    {
        public ConfirmEmailCommandValidator()
        {
            ApplyValidationRules();
        }

        private void ApplyValidationRules()
        {
            RuleFor(x => x.Email).Required();
            RuleFor(x => x.Code).Required();

            When(x => !string.IsNullOrWhiteSpace(x.Email), () =>
            {
                RuleFor(x => x.Email).ApplyEmailRules();
            });
        }
    }
}
