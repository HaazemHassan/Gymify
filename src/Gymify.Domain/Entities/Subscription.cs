using Gymify.Domain.Common.Enums;
using Gymify.Domain.Entities.Base;

namespace Gymify.Domain.Entities
{
    public sealed class Subscription : FullAuditableEntity<int>
    {
        private Subscription()
        {
        }

        public Subscription(
            int customerId,
            int packageId,
            string packageName,
            decimal packagePrice,
            int packageDuration,
            DateTimeOffset startDate,
            DateTimeOffset endDate,
            PaymentMethod paymentMethod,
            SubscriptionType type = SubscriptionType.Regular,
            int? trainerId = null,
            string? packageDescription = null,
            string? notes = null)
        {
            CustomerId = customerId;
            PackageId = packageId;
            PackageName = packageName;
            PackagePrice = packagePrice;
            PackageDuration = packageDuration;
            StartDate = startDate;
            EndDate = endDate;
            PaymentMethod = paymentMethod;
            Type = type;
            TrainerId = type == SubscriptionType.Private ? trainerId : null;
            PackageDescription = packageDescription;
            Notes = notes;
        }

        public int CustomerId { get; private set; }
        public Customer Customer { get; private set; } = null!;

        public int PackageId { get; private set; }
        public Package Package { get; private set; } = null!;

        // Snapshot of package details at time of subscription
        public string PackageName { get; private set; } = string.Empty;
        public decimal PackagePrice { get; private set; }
        public int PackageDuration { get; private set; }
        public string? PackageDescription { get; private set; }

        public DateTimeOffset StartDate { get; private set; }
        public DateTimeOffset EndDate { get; private set; }

        public PaymentMethod PaymentMethod { get; private set; }
        public SubscriptionType Type { get; private set; } = SubscriptionType.Regular;
        public int? TrainerId { get; private set; }
        public Trainer? Trainer { get; private set; }
        public string? Notes { get; private set; }

        public bool IsActive(DateTimeOffset now) => StartDate <= now && EndDate >= now;
    }
}
