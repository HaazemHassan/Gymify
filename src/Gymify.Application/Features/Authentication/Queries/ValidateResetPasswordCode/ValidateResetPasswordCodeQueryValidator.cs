using FluentValidation;
using Gymify.Application.Common;

namespace Gymify.Application.Features.Authentication.Queries.ValidateResetPasswordCode
{
    public class ValidateResetPasswordCodeQueryValidator : AbstractValidator<ValidateResetPasswordCodeQuery>
    {
        public ValidateResetPasswordCodeQueryValidator()
        {
            RuleFor(x => x.Email).Required();
            RuleFor(x => x.Code).Required();
        }
    }
}
