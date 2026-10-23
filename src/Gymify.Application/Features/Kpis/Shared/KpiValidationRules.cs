using FluentValidation;

namespace Gymify.Application.Features.Kpis.Shared
{
    public static class KpiValidationRules
    {
        public static IRuleBuilderOptions<T, string?> ApplyKpiNameRules<T>(
            this IRuleBuilder<T, string?> ruleBuilder,
            int minLength = 2,
            int maxLength = 30)
        {
            return ruleBuilder
                .MinimumLength(minLength)
                    .WithMessage($"{{PropertyName}} must be at least {minLength} characters.")
                .MaximumLength(maxLength)
                    .WithMessage($"{{PropertyName}} cannot exceed {maxLength} characters.");
        }

        public static IRuleBuilderOptions<T, string?> ApplyKpiDescriptionRules<T>(
            this IRuleBuilder<T, string?> ruleBuilder,
            int maxLength = 500)
        {
            return ruleBuilder
                .MaximumLength(maxLength)
                    .WithMessage($"{{PropertyName}} cannot exceed {maxLength} characters.");
        }
    }
}
