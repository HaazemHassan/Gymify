using FluentValidation;
using Gymify.Application.Common;
using Gymify.Application.Features.Authentication.Commands.ResendConfirmEmail;

namespace Gymify.Application.Features.Authentication.Commands.ResendConfirmationEmail
{
    public class ResendConfirmationEmailCommandValidator : AbstractValidator<ResendConfirmationEmailCommand>
    {
        public ResendConfirmationEmailCommandValidator()
        {
            ApplyValidationRules();
        }

        private void ApplyValidationRules()
        {
            RuleFor(x => x.Email).Required();

            When(x => !string.IsNullOrEmpty(x.Email), () =>
            {
                RuleFor(x => x.Email).ApplyEmailRules();
            });
        }
    }
}
