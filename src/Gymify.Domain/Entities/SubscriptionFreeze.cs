using Gymify.Domain.Entities.Base;

namespace Gymify.Domain.Entities
{
    public sealed class SubscriptionFreeze : FullAuditableEntity<int>
    {
        private SubscriptionFreeze()
        {
        }

        public SubscriptionFreeze(
            int subscriptionId,
            DateTimeOffset startDate,
            DateTimeOffset endDate,
            string? reason = null)
        {
            SubscriptionId = subscriptionId;
            StartDate = startDate;
            EndDate = endDate;
            Reason = reason;
        }

        public int SubscriptionId { get; private set; }
        public Subscription Subscription { get; private set; } = null!;

        public DateTimeOffset StartDate { get; private set; }
        public DateTimeOffset EndDate { get; private set; }
        public string? Reason { get; private set; }

        public bool IsActive(DateTimeOffset now) => StartDate <= now && EndDate >= now;

        public void EndEarly(DateTimeOffset now)
        {
            EndDate = now;
        }
    }
}
