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
            string? packageDescription,
            PaymentMethod paymentMethod,
            string? notes = null)
        {
            CustomerId = customerId;
            PackageId = packageId;
            PackageName = packageName;
            PackagePrice = packagePrice;
            PackageDuration = packageDuration;
            PackageDescription = packageDescription;
            PaymentMethod = paymentMethod;
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

        public PaymentMethod PaymentMethod { get; private set; }
        public string? Notes { get; private set; }
    }
}
