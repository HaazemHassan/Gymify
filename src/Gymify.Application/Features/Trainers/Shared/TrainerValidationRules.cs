using FluentValidation;

namespace Gymify.Application.Features.Trainers.Shared
{
    public static class TrainerValidationRules
    {
        public static IRuleBuilderOptions<T, int> ApplyYearsOfExperienceRules<T>(this IRuleBuilder<T, int> ruleBuilder) =>
            ruleBuilder.GreaterThanOrEqualTo(0).WithMessage("Years of experience cannot be negative.");

        public static IRuleBuilderOptions<T, int?> ApplyYearsOfExperienceRules<T>(this IRuleBuilder<T, int?> ruleBuilder) =>
            ruleBuilder.GreaterThanOrEqualTo(0).WithMessage("Years of experience cannot be negative.");

        public static IRuleBuilderOptions<T, decimal> ApplySalaryRules<T>(this IRuleBuilder<T, decimal> ruleBuilder) =>
            ruleBuilder.GreaterThanOrEqualTo(0).WithMessage("Salary cannot be negative.");

        public static IRuleBuilderOptions<T, decimal?> ApplySalaryRules<T>(this IRuleBuilder<T, decimal?> ruleBuilder) =>
            ruleBuilder.GreaterThanOrEqualTo(0).WithMessage("Salary cannot be negative.");
    }
}
