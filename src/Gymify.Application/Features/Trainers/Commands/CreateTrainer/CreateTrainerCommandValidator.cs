using FluentValidation;
using Gymify.Application.Common;
using Gymify.Application.Common.Options;
using Gymify.Application.Features.Trainers.Shared;
using Gymify.Application.Features.Users.Shared;
using Gymify.Application.ServicesContracts.Infrastructure;
using Microsoft.Extensions.Options;

namespace Gymify.Application.Features.Trainers.Commands.CreateTrainer
{
    public class CreateTrainerCommandValidator : AbstractValidator<CreateTrainerCommand>
    {
        private readonly IPhoneNumberService _phoneNumberService;
        private readonly AppPasswordOptions _passwordSettings;

        public CreateTrainerCommandValidator(IOptions<AppPasswordOptions> passwordSettings, IPhoneNumberService phoneNumberService)
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

            RuleFor(x => x.Salary).ApplySalaryRules();
            RuleFor(x => x.YearsOfExperience).ApplyYearsOfExperienceRules();
        }
    }
}
