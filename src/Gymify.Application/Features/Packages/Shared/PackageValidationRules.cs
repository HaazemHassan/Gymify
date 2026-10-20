using FluentValidation;

namespace Gymify.Application.Features.Packages.Shared
{
    public static class PackageValidationRules
    {
        public static IRuleBuilderOptions<T, string?> ApplyPackageNameRules<T>(
            this IRuleBuilder<T, string?> ruleBuilder)
        {
            return ruleBuilder
                .MaximumLength(200)
                .WithMessage("{PropertyName} cannot exceed 200 characters");
        }

        public static IRuleBuilderOptions<T, decimal> ApplyPriceRules<T>(
            this IRuleBuilder<T, decimal> ruleBuilder)
        {
            return ruleBuilder
                .GreaterThan(0)
                .WithMessage("Price must be greater than 0");
        }

        public static IRuleBuilderOptions<T, int> ApplyDurationRules<T>(
            this IRuleBuilder<T, int> ruleBuilder)
        {
            return ruleBuilder
                .GreaterThan(0)
                .WithMessage("Duration must be greater than 0");
        }

        public static IRuleBuilderOptions<T, string?> ApplyDescriptionRules<T>(
            this IRuleBuilder<T, string?> ruleBuilder)
        {
            return ruleBuilder
                .MaximumLength(1000)
                .WithMessage("{PropertyName} cannot exceed 1000 characters");
        }
    }
}
