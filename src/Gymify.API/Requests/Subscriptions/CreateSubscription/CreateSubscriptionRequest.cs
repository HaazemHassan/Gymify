using Gymify.Domain.Common.Enums;

namespace Gymify.API.Requests.Subscriptions.CreateSubscription
{
    public class CreateSubscriptionRequest
    {
        public int CustomerId { get; set; }
        public int PackageId { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public string? Notes { get; set; }
    }
}
