using FluentValidation;
using Gymify.Domain.Common.Enums;

namespace Gymify.Application.Features.Subscriptions.Shared
{
    public static class SubscriptionValidationRules
    {
        public static IRuleBuilderOptions<T, int> ApplyCustomerIdRules<T>(this IRuleBuilder<T, int> ruleBuilder)
        {
            return ruleBuilder
                .GreaterThan(0)
                .WithMessage("Customer ID must be greater than 0.");
        }

        public static IRuleBuilderOptions<T, int> ApplyPackageIdRules<T>(this IRuleBuilder<T, int> ruleBuilder)
        {
            return ruleBuilder
                .GreaterThan(0)
                .WithMessage("Package ID must be greater than 0.");
        }

        public static IRuleBuilderOptions<T, int?> ApplyTrainerIdRules<T>(this IRuleBuilder<T, int?> ruleBuilder)
        {
            return ruleBuilder
                .GreaterThan(0)
                .When(x => x != null)
                .WithMessage("Trainer ID must be greater than 0.");
        }

        public static IRuleBuilderOptions<T, PaymentMethod> ApplyPaymentMethodRules<T>(this IRuleBuilder<T, PaymentMethod> ruleBuilder)
        {
            return ruleBuilder
                .IsInEnum()
                .WithMessage("Invalid payment method. Allowed values are Cash, MobileWallet, InstaPay, Fawry, or Other.");
        }

        public static IRuleBuilderOptions<T, string?> ApplyNotesRules<T>(this IRuleBuilder<T, string?> ruleBuilder)
        {
            return ruleBuilder
                .MaximumLength(2000)
                .WithMessage("Notes must not exceed 2000 characters.");
        }
    }
}
