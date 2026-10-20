using FluentValidation;
using Gymify.Application.Common;
using Gymify.Application.Features.Users.Common;
using Gymify.Application.ServicesContracts.Infrastructure;

namespace Gymify.Application.Features.Users.Commands.UpdateProfile
{
    public class UpdateProfileCommandValidator : AbstractValidator<UpdateProfileCommand>
    {

        private readonly IPhoneNumberService _phoneNumberService;
        public UpdateProfileCommandValidator(IPhoneNumberService phoneNumberService)
        {
            _phoneNumberService = phoneNumberService;

            ApplyCustomValidations();
            ApplyValidationRules();
        }

        private void ApplyValidationRules()
        {
            When(x => !string.IsNullOrWhiteSpace(x.Name), () =>
            {
                RuleFor(x => x.Name).ApplyUserNameRules();
            });

            When(x => !string.IsNullOrWhiteSpace(x.PhoneNumber), () =>
            {
                RuleFor(x => x.PhoneNumber).ApplyPhoneNumberRules(_phoneNumberService);
            });


        }

        private void ApplyCustomValidations()
        {
            RuleFor(x => x)
               .Must(HaveAtLeastOneNonNullProperty)
               .WithMessage("Nothing to change.");
        }

        private bool HaveAtLeastOneNonNullProperty(UpdateProfileCommand command)
        {
            return command.Name != null ||
                   command.PhoneNumber != null;
        }
    }
}
