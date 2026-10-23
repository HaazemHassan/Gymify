using FluentValidation;
using Gymify.Application.Common;
using Gymify.Application.Features.Kpis.Shared;

namespace Gymify.Application.Features.Kpis.Commands.CreateKpi
{
    public class CreateKpiCommandValidator : AbstractValidator<CreateKpiCommand>
    {
        public CreateKpiCommandValidator()
        {
            ApplyValidationRules();
        }

        private void ApplyValidationRules()
        {
            RuleFor(x => x.Name).Required();

            When(x => !string.IsNullOrWhiteSpace(x.Name), () =>
            {
                RuleFor(x => x.Name).ApplyKpiNameRules();
            });

            When(x => !string.IsNullOrWhiteSpace(x.Description), () =>
            {
                RuleFor(x => x.Description).ApplyKpiDescriptionRules();
            });
        }
    }
}
