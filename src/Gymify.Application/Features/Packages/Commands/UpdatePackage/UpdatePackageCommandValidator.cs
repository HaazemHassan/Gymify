using FluentValidation;
using Gymify.Application.Common;
using Gymify.Application.Features.Packages.Shared;

namespace Gymify.Application.Features.Packages.Commands.UpdatePackage
{
    public class UpdatePackageCommandValidator : AbstractValidator<UpdatePackageCommand>
    {
        public UpdatePackageCommandValidator()
        {
            ApplyValidationRules();
        }

        private void ApplyValidationRules()
        {
            RuleFor(x => x.Id).Required();

            When(x => x.Name != null, () =>
            {
                RuleFor(x => x.Name!).ApplyPackageNameRules();
            });

            When(x => x.Price.HasValue, () =>
            {
                RuleFor(x => x.Price!.Value).ApplyPriceRules();
            });

            When(x => x.Duration.HasValue, () =>
            {
                RuleFor(x => x.Duration!.Value).ApplyDurationRules();
            });

            When(x => !string.IsNullOrWhiteSpace(x.Description), () =>
            {
                RuleFor(x => x.Description).ApplyDescriptionRules();
            });

            When(x => x.Type.HasValue, () =>
            {
                RuleFor(x => x.Type!.Value).ApplyPackageTypeRules();
            });

            When(x => x.TrainerId.HasValue, () =>
            {
                RuleFor(x => x.TrainerId).ApplyTrainerIdRules();
            });

            RuleFor(x => x)
                .Must(HaveAtLeastOneNonNullProperty)
                .WithMessage("At least one property must be provided to update");
        }

        private bool HaveAtLeastOneNonNullProperty(UpdatePackageCommand command)
        {
            return command.Name != null ||
                   command.Price.HasValue ||
                   command.Duration.HasValue ||
                   command.Description != null ||
                   command.IsActive.HasValue ||
                   command.Type.HasValue ||
                   command.TrainerId.HasValue;
        }
    }
}
