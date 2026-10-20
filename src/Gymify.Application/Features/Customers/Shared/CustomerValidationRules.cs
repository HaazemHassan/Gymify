using FluentValidation;
using Gymify.Domain.Common.Enums;

namespace Gymify.Application.Features.Customers.Shared
{
    public static class CustomerValidationRules
    {
        public static IRuleBuilderOptions<T, int> ApplyCustomerIdRules<T>(this IRuleBuilder<T, int> ruleBuilder)
        {
            return ruleBuilder
                .GreaterThan(0)
                .WithMessage("{PropertyName} must be greater than 0.");
        }

        public static IRuleBuilderOptions<T, double> ApplyHeightRules<T>(this IRuleBuilder<T, double> ruleBuilder)
        {
            return ruleBuilder
                .GreaterThan(0)
                .WithMessage("{PropertyName} must be greater than 0.");
        }

        public static IRuleBuilderOptions<T, double> ApplyWeightRules<T>(this IRuleBuilder<T, double> ruleBuilder)
        {
            return ruleBuilder
                .GreaterThan(0)
                .WithMessage("{PropertyName} must be greater than 0.");
        }

        public static IRuleBuilderOptions<T, int> ApplyAgeRules<T>(this IRuleBuilder<T, int> ruleBuilder)
        {
            return ruleBuilder
                .InclusiveBetween(5, 120)
                .WithMessage("{PropertyName} must be between 5 and 120.");
        }

        public static IRuleBuilderOptions<T, double> ApplyMusclePercentageRules<T>(this IRuleBuilder<T, double> ruleBuilder)
        {
            return ruleBuilder
                .InclusiveBetween(0, 100)
                .WithMessage("{PropertyName} must be between 0 and 100.");
        }

        public static IRuleBuilderOptions<T, double> ApplyFatPercentageRules<T>(this IRuleBuilder<T, double> ruleBuilder)
        {
            return ruleBuilder
                .InclusiveBetween(0, 100)
                .WithMessage("{PropertyName} must be between 0 and 100.");
        }

        public static IRuleBuilderOptions<T, ApprovalStatus> ApplyApprovalStatusRules<T>(this IRuleBuilder<T, ApprovalStatus> ruleBuilder)
        {
            return ruleBuilder
                .IsInEnum()
                .WithMessage("Invalid approval status.");
        }

        public static IRuleBuilderOptions<T, string?> ApplyCodeRules<T>(this IRuleBuilder<T, string?> ruleBuilder)
        {
            return ruleBuilder
                .MaximumLength(50)
                .WithMessage("{PropertyName} cannot exceed 50 characters.");
        }

        public static IRuleBuilderOptions<T, string?> ApplyInjuriesRules<T>(this IRuleBuilder<T, string?> ruleBuilder)
        {
            return ruleBuilder
                .MaximumLength(1000)
                .WithMessage("{PropertyName} cannot exceed 1000 characters.");
        }

        public static IRuleBuilderOptions<T, string?> ApplyChronicDiseasesRules<T>(this IRuleBuilder<T, string?> ruleBuilder)
        {
            return ruleBuilder
                .MaximumLength(1000)
                .WithMessage("{PropertyName} cannot exceed 1000 characters.");
        }

        public static IRuleBuilderOptions<T, string?> ApplyNotesRules<T>(this IRuleBuilder<T, string?> ruleBuilder)
        {
            return ruleBuilder
                .MaximumLength(1000)
                .WithMessage("{PropertyName} cannot exceed 1000 characters.");
        }
    }
}
