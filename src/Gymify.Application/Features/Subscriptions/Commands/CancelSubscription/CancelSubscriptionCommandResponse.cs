namespace Gymify.Application.Features.Subscriptions.Commands.CancelSubscription
{
    public class CancelSubscriptionCommandResponse
    {
        public int SubscriptionId { get; set; }
        public DateTimeOffset CancelledAt { get; set; }
        public string? CancellationReason { get; set; }
    }
}
