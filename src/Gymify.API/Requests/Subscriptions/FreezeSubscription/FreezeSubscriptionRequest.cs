namespace Gymify.API.Requests.Subscriptions.FreezeSubscription
{
    public class FreezeSubscriptionRequest
    {
        public DateTimeOffset EndDate { get; set; }
        public string? Reason { get; set; }
    }
}
