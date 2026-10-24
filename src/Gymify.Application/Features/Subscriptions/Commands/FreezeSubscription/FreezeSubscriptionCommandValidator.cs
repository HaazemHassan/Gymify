using FluentValidation;

namespace Gymify.Application.Features.Subscriptions.Commands.FreezeSubscription
{
    public class FreezeSubscriptionCommandValidator : AbstractValidator<FreezeSubscriptionCommand>
    {
        public FreezeSubscriptionCommandValidator()
        {
            RuleFor(x => x.SubscriptionId)
                .GreaterThan(0)
                .WithMessage("Subscription ID must be greater than zero.");

            RuleFor(x => x.EndDate)
                .NotEmpty()
                .WithMessage("Freeze end date is required.");

            RuleFor(x => x.Reason)
                .MaximumLength(500)
                .When(x => !string.IsNullOrEmpty(x.Reason))
                .WithMessage("Reason cannot exceed 500 characters.");
        }
    }
}
