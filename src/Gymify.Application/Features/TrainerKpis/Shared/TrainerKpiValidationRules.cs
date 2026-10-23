using FluentValidation;

namespace Gymify.Application.Features.TrainerKpis.Shared
{
    public static class TrainerKpiValidationRules
    {
        public static IRuleBuilderOptions<T, decimal> ApplyKpiScoreRules<T>(
            this IRuleBuilder<T, decimal> ruleBuilder,
            decimal min = 1,
            decimal max = 5)
        {
            return ruleBuilder
                .InclusiveBetween(min, max)
                .WithMessage($"{{PropertyName}} must be between {min} and {max}.");
        }

        public static IRuleBuilderOptions<T, string?> ApplyCommentRules<T>(
            this IRuleBuilder<T, string?> ruleBuilder,
            int maxLength = 500)
        {
            return ruleBuilder
                .MaximumLength(maxLength)
                .WithMessage($"{{PropertyName}} cannot exceed {maxLength} characters.");
        }
    }
}
