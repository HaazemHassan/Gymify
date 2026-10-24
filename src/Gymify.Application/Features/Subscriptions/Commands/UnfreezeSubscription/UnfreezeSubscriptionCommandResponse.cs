namespace Gymify.Application.Features.Subscriptions.Commands.UnfreezeSubscription
{
    public class UnfreezeSubscriptionCommandResponse
    {
        public int SubscriptionId { get; set; }
        public DateTimeOffset NewSubscriptionEndDate { get; set; }
    }
}
