using FluentValidation;
using Gymify.Domain.Common.Enums;

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

        public static IRuleBuilderOptions<T, Gymify.Domain.Common.Enums.PackageType> ApplyPackageTypeRules<T>(
            this IRuleBuilder<T, Gymify.Domain.Common.Enums.PackageType> ruleBuilder)
        {
            return ruleBuilder
                .IsInEnum()
                .WithMessage("Invalid package type");
        }

        public static IRuleBuilderOptions<T, int?> ApplyTrainerIdRules<T>(
            this IRuleBuilder<T, int?> ruleBuilder)
        {
            return ruleBuilder
                .GreaterThan(0)
                .WithMessage("Trainer ID must be greater than 0");
        }
    }
}
