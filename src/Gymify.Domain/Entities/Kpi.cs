using Gymify.Domain.Entities.Base;

namespace Gymify.Domain.Entities
{
    public sealed class Kpi : FullAuditableEntity<int>
    {
        private Kpi()
        {
        }

        public Kpi(string name, string? description = null, bool isActive = true)
        {
            Name = name;
            Description = description;
            IsActive = isActive;
        }

        public string Name { get; private set; } = string.Empty;
        public string? Description { get; private set; }
        public bool IsActive { get; private set; } = true;

        public void Update(string? name = null, string? description = null, bool? isActive = null)
        {
            if (!string.IsNullOrWhiteSpace(name))
            {
                Name = name;
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

        public void SetActive(bool isActive)
        {
            IsActive = isActive;
        }
    }
}
