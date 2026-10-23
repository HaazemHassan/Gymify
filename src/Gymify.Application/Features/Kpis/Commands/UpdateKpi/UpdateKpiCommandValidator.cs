using FluentValidation;
using Gymify.Application.Features.Kpis.Shared;

namespace Gymify.Application.Features.Kpis.Commands.UpdateKpi
{
    public class UpdateKpiCommandValidator : AbstractValidator<UpdateKpiCommand>
    {
        public UpdateKpiCommandValidator()
        {
            ApplyCustomValidations();
            ApplyValidationRules();
        }

        private void ApplyValidationRules()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("{PropertyName} must be greater than 0.");

            When(x => !string.IsNullOrWhiteSpace(x.Name), () =>
            {
                RuleFor(x => x.Name).ApplyKpiNameRules();
            });

            When(x => x.Description is not null, () =>
            {
                RuleFor(x => x.Description).ApplyKpiDescriptionRules();
            });
        }

        private void ApplyCustomValidations()
        {
            RuleFor(x => x)
                .Must(HaveAtLeastOneUpdatableField)
                .WithMessage("At least one property must be provided to update.");
        }

        private bool HaveAtLeastOneUpdatableField(UpdateKpiCommand command)
        {
            return command.Name != null ||
                   command.Description != null ||
                   command.IsActive.HasValue;
        }
    }
}
