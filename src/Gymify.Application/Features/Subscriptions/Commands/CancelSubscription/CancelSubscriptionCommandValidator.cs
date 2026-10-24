using FluentValidation;

namespace Gymify.Application.Features.Subscriptions.Commands.CancelSubscription
{
    public class CancelSubscriptionCommandValidator : AbstractValidator<CancelSubscriptionCommand>
    {
        public CancelSubscriptionCommandValidator()
        {
            RuleFor(x => x.SubscriptionId)
                .GreaterThan(0)
                .WithMessage("Subscription ID must be greater than zero.");

            RuleFor(x => x.Reason)
                .MaximumLength(500)
                .When(x => !string.IsNullOrEmpty(x.Reason))
                .WithMessage("Cancellation reason cannot exceed 500 characters.");
        }
    }
}
