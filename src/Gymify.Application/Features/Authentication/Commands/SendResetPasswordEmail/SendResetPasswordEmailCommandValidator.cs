using FluentValidation;
using Gymify.Application.Common;

namespace Gymify.Application.Features.Authentication.Commands.SendResetPasswordEmail
{
    public class SendResetPasswordEmailCommandValidator : AbstractValidator<SendResetPasswordEmailCommand>
    {
        public SendResetPasswordEmailCommandValidator()
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
