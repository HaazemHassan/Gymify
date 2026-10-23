using FluentValidation;
using Gymify.Application.Common;
using Gymify.Application.Common.Options;
using Gymify.Application.Features.Customers.Shared;
using Gymify.Application.Features.Users.Shared;
using Gymify.Application.ServicesContracts.Infrastructure;
using Microsoft.Extensions.Options;

namespace Gymify.Application.Features.Customers.Commands.CreateCustomer
{
    public class CreateCustomerCommandValidator : AbstractValidator<CreateCustomerCommand>
    {
        private readonly IPhoneNumberService _phoneNumberService;
        private readonly AppPasswordOptions _passwordSettings;

        public CreateCustomerCommandValidator(IOptions<AppPasswordOptions> passwordSettings, IPhoneNumberService phoneNumberService)
        {
            _passwordSettings = passwordSettings.Value;
            _phoneNumberService = phoneNumberService;

            ApplyValidationRules();
        }

        private void ApplyValidationRules()
        {
            RuleFor(x => x.Name).Required();
            RuleFor(x => x.Email).Required();
            RuleFor(x => x.PhoneNumber).Required();
            RuleFor(x => x.Password).Required();
            RuleFor(x => x.ConfirmPassword).Required();

            When(x => !string.IsNullOrWhiteSpace(x.Name), () =>
            {
                RuleFor(x => x.Name).ApplyUserFullNameRules();
            });

            When(x => !string.IsNullOrWhiteSpace(x.Email), () =>
            {
                RuleFor(x => x.Email).ApplyEmailRules();
            });

            When(x => !string.IsNullOrWhiteSpace(x.PhoneNumber), () =>
            {
                RuleFor(x => x.PhoneNumber).ApplyPhoneNumberRules(_phoneNumberService);
            });

            When(x => !string.IsNullOrWhiteSpace(x.Password), () =>
            {
                RuleFor(x => x.Password).ApplyPasswordRules(_passwordSettings);
            });

            When(x => !string.IsNullOrWhiteSpace(x.Password) && !string.IsNullOrWhiteSpace(x.ConfirmPassword), () =>
            {
                RuleFor(x => x.ConfirmPassword).ApplyConfirmPasswordRules(x => x.Password);
            });

            When(x => x.Height.HasValue, () =>
            {
                RuleFor(x => x.Height!.Value).ApplyHeightRules();
            });

            When(x => x.Weight.HasValue, () =>
            {
                RuleFor(x => x.Weight!.Value).ApplyWeightRules();
            });

            When(x => x.Age.HasValue, () =>
            {
                RuleFor(x => x.Age!.Value).ApplyAgeRules();
            });

            When(x => x.MusclePercentage.HasValue, () =>
            {
                RuleFor(x => x.MusclePercentage!.Value).ApplyMusclePercentageRules();
            });

            When(x => x.FatPercentage.HasValue, () =>
            {
                RuleFor(x => x.FatPercentage!.Value).ApplyFatPercentageRules();
            });

            When(x => !string.IsNullOrWhiteSpace(x.Code), () =>
            {
                RuleFor(x => x.Code).ApplyCodeRules();
            });

            When(x => !string.IsNullOrWhiteSpace(x.Injuries), () =>
            {
                RuleFor(x => x.Injuries).ApplyInjuriesRules();
            });

            When(x => !string.IsNullOrWhiteSpace(x.ChronicDiseases), () =>
            {
                RuleFor(x => x.ChronicDiseases).ApplyChronicDiseasesRules();
            });

            When(x => !string.IsNullOrWhiteSpace(x.Notes), () =>
            {
                RuleFor(x => x.Notes).ApplyNotesRules();
            });
        }
    }
}
