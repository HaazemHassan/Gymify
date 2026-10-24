using Ardalis.Specification;
using Gymify.Domain.Entities;

namespace Gymify.Application.Features.Trainers.Specifications
{
    public class TrainersFilteredSpec : Specification<Trainer>
    {
        public TrainersFilteredSpec(string? search, bool? isAvailable = null, bool? isActive = null)
        {
            if (isAvailable.HasValue)
            {
                Query.Where(t => t.IsAvailable == isAvailable.Value);
            }

            if (isActive.HasValue)
            {
                Query.Where(t => t.Employee.DomainUser.IsActive == isActive.Value);
            }

            if (!string.IsNullOrWhiteSpace(search))
            {
                var trimmed = search.Trim();
                Query.Where(t => t.Employee.DomainUser.Name.Contains(trimmed));
            }
        }
    }
}
