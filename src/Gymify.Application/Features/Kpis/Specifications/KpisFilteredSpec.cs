using Ardalis.Specification;
using Gymify.Domain.Entities;

namespace Gymify.Application.Features.Kpis.Specifications
{
    public class KpisFilteredSpec : Specification<Kpi>
    {
        public KpisFilteredSpec(string? search = null, bool? isActive = null)
        {
            if (isActive.HasValue)
            {
                Query.Where(k => k.IsActive == isActive.Value);
            }

            if (!string.IsNullOrWhiteSpace(search))
            {
                var term = search.Trim();
                Query.Where(k => k.Name.Contains(term) || (k.Description != null && k.Description.Contains(term)));
            }
        }
    }
}
