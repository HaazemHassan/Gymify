using Gymify.Domain.Entities.Base;

namespace Gymify.Domain.Entities
{
    public sealed class Package : FullAuditableEntity<int>
    {
        private Package()
        {
        }

        public Package(
            string name,
            decimal price,
            int duration,
            string? description = null,
            bool isActive = true)
        {
            Name = name;
            Price = price;
            Duration = duration;
            Description = description;
            IsActive = isActive;
        }

        public string Name { get; private set; } = string.Empty;
        public decimal Price { get; private set; }
        public string? Description { get; private set; }
        public int Duration { get; private set; }
        public bool IsActive { get; private set; } = true;

        public ICollection<Subscription> Subscriptions { get; private set; } = new List<Subscription>();

        public void Update(
            string? name = null,
            decimal? price = null,
            int? duration = null,
            string? description = null,
            bool? isActive = null)
        {

            if (!string.IsNullOrWhiteSpace(name))
            {
                Name = name;
            }

            if (price.HasValue)
            {
                Price = price.Value;
            }
            if (duration.HasValue)
            {
                Duration = duration.Value;
            }

            if (description is not null)
            {
                Description = description;
            }

            if (isActive.HasValue)
            {
                IsActive = isActive.Value;
            }
        }

        public void Deactivate()
        {
            IsActive = false;
        }

        public void Activate()
        {
            IsActive = true;
        }
    }
}
