using FluentValidation;
using Gymify.Application.Common;
using Gymify.Application.Features.Customers.Shared;
using Gymify.Application.Features.Users.Common;
using Gymify.Application.ServicesContracts.Infrastructure;

namespace Gymify.Application.Features.Customers.Commands.UpdateCustomer
{
    public class UpdateCustomerCommandValidator : AbstractValidator<UpdateCustomerCommand>
    {
        private readonly IPhoneNumberService _phoneNumberService;

        public UpdateCustomerCommandValidator(IPhoneNumberService phoneNumberService)
        {
            _phoneNumberService = phoneNumberService;

            ApplyCustomValidations();
            ApplyValidationRules();
        }

        private void ApplyValidationRules()
        {
            RuleFor(x => x.CustomerId).ApplyCustomerIdRules();

            When(x => !string.IsNullOrWhiteSpace(x.Name), () =>
            {
                RuleFor(x => x.Name).ApplyUserNameRules();
            });

            When(x => !string.IsNullOrWhiteSpace(x.PhoneNumber), () =>
            {
                RuleFor(x => x.PhoneNumber).ApplyPhoneNumberRules(_phoneNumberService);
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

            When(x => x.ApprovalStatus.HasValue, () =>
            {
                RuleFor(x => x.ApprovalStatus!.Value).ApplyApprovalStatusRules();
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

        private void ApplyCustomValidations()
        {
            RuleFor(x => x)
                .Must(HaveAtLeastOneUpdatableField)
                .WithMessage("At least one property must be provided to update.");
        }

        private bool HaveAtLeastOneUpdatableField(UpdateCustomerCommand command)
        {
            return command.Name != null ||
                   command.PhoneNumber != null ||
                   command.IsActive.HasValue ||
                   command.Code != null ||
                   command.Height.HasValue ||
                   command.Weight.HasValue ||
                   command.Age.HasValue ||
                   command.Injuries != null ||
                   command.ChronicDiseases != null ||
                   command.MusclePercentage.HasValue ||
                   command.FatPercentage.HasValue ||
                   command.Notes != null ||
                   command.ApprovalStatus.HasValue;
        }
    }
}
