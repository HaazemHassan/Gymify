using Gymify.Domain.Common.Enums;
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
            bool isActive = true,
            PackageType type = PackageType.Regular,
            int? trainerId = null)
        {
            Name = name;
            Price = price;
            Duration = duration;
            Description = description;
            IsActive = isActive;
            Type = type;
            TrainerId = type == PackageType.Private ? trainerId : null;
        }

        public string Name { get; private set; } = string.Empty;
        public decimal Price { get; private set; }
        public string? Description { get; private set; }
        public int Duration { get; private set; }
        public bool IsActive { get; private set; } = true;
        public PackageType Type { get; private set; } = PackageType.Regular;
        public int? TrainerId { get; private set; }
        public Trainer? Trainer { get; private set; }

        public ICollection<Subscription> Subscriptions { get; private set; } = new List<Subscription>();

        public void Update(
            string? name = null,
            decimal? price = null,
            int? duration = null,
            string? description = null,
            bool? isActive = null,
            PackageType? type = null,
            int? trainerId = null)
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

            if (type.HasValue)
            {
                Type = type.Value;
                TrainerId = type.Value == PackageType.Private ? trainerId : null;
            }
            else if (trainerId.HasValue && Type == PackageType.Private)
            {
                TrainerId = trainerId.Value;
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
