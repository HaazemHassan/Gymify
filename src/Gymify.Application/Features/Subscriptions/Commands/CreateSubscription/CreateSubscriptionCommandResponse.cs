using Gymify.Domain.Common.Enums;

namespace Gymify.Application.Features.Subscriptions.Commands.CreateSubscription
{
    public class CreateSubscriptionCommandResponse
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int PackageId { get; set; }
        public int? TrainerId { get; set; }
        public string PackageName { get; set; } = string.Empty;
        public decimal PackagePrice { get; set; }
        public int PackageDuration { get; set; }
        public string? PackageDescription { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset EndDate { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public SubscriptionType Type { get; set; }
        public string? Notes { get; set; }
    }
}
