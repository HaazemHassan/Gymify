using Gymify.Domain.Common.Enums;

namespace Gymify.Application.Features.Subscriptions.Queries.GetSubscriptionById
{
    public class GetSubscriptionByIdQueryResponse
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string? CustomerPhone { get; set; }

        public int PackageId { get; set; }
        public string PackageName { get; set; } = string.Empty;
        public decimal PackagePrice { get; set; }
        public int PackageDuration { get; set; }
        public string? PackageDescription { get; set; }

        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset EndDate { get; set; }

        public PaymentMethod PaymentMethod { get; set; }
        public SubscriptionType Type { get; set; }

        public int? TrainerId { get; set; }
        public string? TrainerName { get; set; }

        public SubscriptionStatus Status { get; set; }
        public bool IsCancelled { get; set; }
        public DateTimeOffset? CancelledAt { get; set; }
        public string? CancellationReason { get; set; }

        public string? Notes { get; set; }
        public DateTimeOffset CreatedAt { get; set; }

        public List<SubscriptionFreezeDetailsDto> Freezes { get; set; } = [];
    }

    public class SubscriptionFreezeDetailsDto
    {
        public int Id { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset EndDate { get; set; }
        public string? Reason { get; set; }
        public bool IsActive { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public int Days => (int)Math.Max(1, Math.Ceiling((EndDate - StartDate).TotalDays));
    }
}
