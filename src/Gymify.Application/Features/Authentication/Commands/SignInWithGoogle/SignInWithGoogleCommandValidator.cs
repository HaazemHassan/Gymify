using FluentValidation;

namespace Gymify.Application.Features.Authentication.Commands.SignInWithGoogle
{
    public class SignInWithGoogleCommandValidator : AbstractValidator<SignInWithGoogleCommand>
    {
        public SignInWithGoogleCommandValidator()
        {
            ApplyValidationRules();
        }

        private void ApplyValidationRules()
        {
            RuleFor(x => x.IdToken)
                .NotEmpty().WithMessage("Google ID token is required.")
                .MinimumLength(100).WithMessage("Invalid Google ID token format.");

            RuleFor(x => x.UserType)
                .IsInEnum().WithMessage("Invalid user type.");
        }
    }

}
