using FluentValidation;

namespace Gymify.Application.Features.Lockers.Shared
{
    public static class LockerValidationRules
    {
        public static IRuleBuilderOptions<T, int> ApplyLockerNumberRules<T>(
            this IRuleBuilder<T, int> ruleBuilder)
        {
            return ruleBuilder
                .GreaterThan(0)
                .WithMessage("{PropertyName} must be greater than zero");
        }
    }
}
