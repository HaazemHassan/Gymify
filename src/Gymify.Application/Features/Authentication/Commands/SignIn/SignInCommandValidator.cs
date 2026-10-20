using FluentValidation;
using Microsoft.Extensions.Options;
using Gymify.Application.Common;
using Gymify.Application.Common.Options;
using Gymify.Application.Features.Users.Common;

namespace Gymify.Application.Features.Authentication.Commands.SignIn
{
    public class SignInCommandValidator : AbstractValidator<SignInWithPasswordCommand>
    {

        private readonly AppPasswordOptions _passwordOptions;
        public SignInCommandValidator(IOptions<AppPasswordOptions> passwordOptions)
        {
            _passwordOptions = passwordOptions.Value;
            ApplyValidationRules();
        }

        private void ApplyValidationRules()
        {
            RuleFor(x => x.Email).Required();
            RuleFor(x => x.Password).Required();
            RuleFor(x => x.UserType).IsInEnum();

            When(x => !string.IsNullOrWhiteSpace(x.Email), () =>
            {
                RuleFor(x => x.Email).ApplyEmailRules();
            });
        }
    }
}
