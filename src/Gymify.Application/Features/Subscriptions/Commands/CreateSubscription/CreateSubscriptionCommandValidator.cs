using FluentValidation;
using Gymify.Application.Features.Subscriptions.Shared;

namespace Gymify.Application.Features.Subscriptions.Commands.CreateSubscription
{
    public class CreateSubscriptionCommandValidator : AbstractValidator<CreateSubscriptionCommand>
    {
        public CreateSubscriptionCommandValidator()
        {
            ApplyValidationRules();
        }

        public void ApplyValidationRules()
        {
            RuleFor(x => x.CustomerId).ApplyCustomerIdRules();
            RuleFor(x => x.PackageId).ApplyPackageIdRules();
            RuleFor(x => x.TrainerId).ApplyTrainerIdRules();
            RuleFor(x => x.PaymentMethod).ApplyPaymentMethodRules();
            RuleFor(x => x.Notes).ApplyNotesRules();
        }
    }
}
