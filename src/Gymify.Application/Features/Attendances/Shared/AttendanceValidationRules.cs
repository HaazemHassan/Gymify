using FluentValidation;

namespace Gymify.Application.Features.Attendances.Shared
{
    public static class AttendanceValidationRules
    {
        public static IRuleBuilderOptions<T, string> ApplyCustomerCodeRules<T>(
            this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .NotEmpty()
                .WithMessage("{PropertyName} is required")
                .MaximumLength(50)
                .WithMessage("{PropertyName} cannot exceed 50 characters");
        }

        public static IRuleBuilderOptions<T, int> ApplyLockerIdRules<T>(
            this IRuleBuilder<T, int> ruleBuilder)
        {
            return ruleBuilder
                .GreaterThan(0)
                .WithMessage("LockerId must be greater than 0");
        }
    }
}
