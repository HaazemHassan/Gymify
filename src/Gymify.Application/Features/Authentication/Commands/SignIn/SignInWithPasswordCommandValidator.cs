using FluentValidation;
using Microsoft.Extensions.Options;
using Gymify.Application.Common;
using Gymify.Application.Common.Options;
using Gymify.Application.Features.Users.Common;

namespace Gymify.Application.Features.Authentication.Commands.SignIn
{
    public class SignInWithPasswordCommandValidator : AbstractValidator<SignInWithPasswordCommand>
    {

        private readonly AppPasswordOptions _passwordOptions;
        public SignInWithPasswordCommandValidator(IOptions<AppPasswordOptions> passwordOptions)
        {
            _passwordOptions = passwordOptions.Value;
            ApplyValidationRules();
        }

        private void ApplyValidationRules()
        {
            RuleFor(x => x.Email).Required();
            RuleFor(x => x.Password).Required();

            When(x => !string.IsNullOrWhiteSpace(x.Email), () =>
            {
                RuleFor(x => x.Email).ApplyEmailRules();
            });
        }
    }
}
