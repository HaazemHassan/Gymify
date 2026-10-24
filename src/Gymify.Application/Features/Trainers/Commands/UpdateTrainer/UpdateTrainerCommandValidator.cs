using FluentValidation;
using Gymify.Application.Common;
using Gymify.Application.Features.Trainers.Shared;
using Gymify.Application.Features.Users.Shared;
using Gymify.Application.ServicesContracts.Infrastructure;

namespace Gymify.Application.Features.Trainers.Commands.UpdateTrainer
{
    public class UpdateTrainerCommandValidator : AbstractValidator<UpdateTrainerCommand>
    {
        private readonly IPhoneNumberService _phoneNumberService;

        public UpdateTrainerCommandValidator(IPhoneNumberService phoneNumberService)
        {
            _phoneNumberService = phoneNumberService;

            ApplyCustomValidations();
            ApplyValidationRules();
        }

        private void ApplyValidationRules()
        {
            RuleFor(x => x.TrainerId)
                .GreaterThan(0)
                .WithMessage("{PropertyName} must be greater than 0.");

            When(x => !string.IsNullOrWhiteSpace(x.Name), () =>
            {
                RuleFor(x => x.Name).ApplyUserFullNameRules();
            });

            When(x => !string.IsNullOrWhiteSpace(x.PhoneNumber), () =>
            {
                RuleFor(x => x.PhoneNumber).ApplyPhoneNumberRules(_phoneNumberService);
            });

            When(x => x.YearsOfExperience.HasValue, () =>
            {
                RuleFor(x => x.YearsOfExperience!.Value).ApplyYearsOfExperienceRules();
            });

            When(x => x.Salary.HasValue, () =>
            {
                RuleFor(x => x.Salary!.Value).ApplySalaryRules();
            });
        }

        private void ApplyCustomValidations()
        {
            RuleFor(x => x)
                .Must(HaveAtLeastOneUpdatableField)
                .WithMessage("At least one property must be provided to update.");
        }

        private bool HaveAtLeastOneUpdatableField(UpdateTrainerCommand command)
        {
            return command.Name != null ||
                   command.PhoneNumber != null ||
                   command.YearsOfExperience.HasValue ||
                   command.Salary.HasValue ||
                   command.IsActive.HasValue ||
                   command.IsAvailable.HasValue;
        }
    }
}
