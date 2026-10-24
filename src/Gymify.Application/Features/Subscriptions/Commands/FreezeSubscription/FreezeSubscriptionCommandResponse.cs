namespace Gymify.Application.Features.Subscriptions.Commands.FreezeSubscription
{
    public class FreezeSubscriptionCommandResponse
    {
        public int FreezeId { get; set; }
        public int SubscriptionId { get; set; }
        public DateTimeOffset FreezeStartDate { get; set; }
        public DateTimeOffset FreezeEndDate { get; set; }
        public DateTimeOffset NewSubscriptionEndDate { get; set; }
        public string? Reason { get; set; }
    }
}
