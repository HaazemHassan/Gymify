using FluentValidation;
using Microsoft.Extensions.Options;
using Gymify.Application.Common;
using Gymify.Application.Common.Options;
using Gymify.Application.Features.Users.Common;
using Gymify.Application.ServicesContracts.Infrastructure;

namespace Gymify.Application.Features.Users.Commands.AddUser
{
    public class AddUserCommandValidator : AbstractValidator<AddUserCommand>
    {
        private readonly IPhoneNumberService _phoneNumberService;
        private readonly AppPasswordOptions _passwordSettings;
        public AddUserCommandValidator(IOptions<AppPasswordOptions> passwordSettings, IPhoneNumberService phoneNumberService)
        {
            _passwordSettings = passwordSettings.Value;
            _phoneNumberService = phoneNumberService;

            ApplyValidationRules();
        }

        private void ApplyValidationRules()
        {
            RuleFor(x => x.Name).Required();
            RuleFor(x => x.Email).Required();
            RuleFor(x => x.Password).Required();
            RuleFor(x => x.ConfirmPassword).Required();
            RuleFor(x => x.PhoneNumber);




            When(x => !string.IsNullOrWhiteSpace(x.Name), () =>
            {
                RuleFor(x => x.Name).ApplyUserNameRules();
            });


            When(x => !string.IsNullOrWhiteSpace(x.Email), () =>
            {
                RuleFor(x => x.Email).ApplyEmailRules();
            });


            When(x => !string.IsNullOrWhiteSpace(x.Password), () =>
            {
                RuleFor(x => x.Password).ApplyPasswordRules(_passwordSettings);
            });


            When(x => !string.IsNullOrWhiteSpace(x.Password) && !string.IsNullOrWhiteSpace(x.ConfirmPassword), () =>
            {
                RuleFor(x => x.ConfirmPassword).ApplyConfirmPasswordRules(x => x.Password);
            });


            When(x => !string.IsNullOrWhiteSpace(x.PhoneNumber), () =>
            {
                RuleFor(x => x.PhoneNumber).ApplyPhoneNumberRules(_phoneNumberService);
            });

            RuleFor(x => x.UserRole)
                .IsInEnum()
                .WithMessage("Invalid role");
        }

    }
}
