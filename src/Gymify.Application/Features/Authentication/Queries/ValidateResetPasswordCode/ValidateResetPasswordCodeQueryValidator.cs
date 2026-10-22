using FluentValidation;
using Gymify.Application.Common;

namespace Gymify.Application.Features.Authentication.Queries.ValidateResetPasswordCode
{
    public class ValidateResetPasswordCodeQueryValidator : AbstractValidator<ValidateResetPasswordCodeQuery>
    {
        public ValidateResetPasswordCodeQueryValidator()
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
