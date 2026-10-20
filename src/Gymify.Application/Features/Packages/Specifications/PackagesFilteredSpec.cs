using Ardalis.Specification;
using Gymify.Domain.Entities;

namespace Gymify.Application.Features.Packages.Specifications
{
    public class PackagesFilteredSpec : Specification<Package>
    {
        public PackagesFilteredSpec(string? search = null, bool? isActive = null)
        {
            if (isActive.HasValue)
            {
                Query.Where(p => p.IsActive == isActive.Value);
            }

            if (!string.IsNullOrWhiteSpace(search))
            {
                var lowerSearch = search.Trim().ToLower();
                Query.Where(p => p.Name.ToLower().Contains(lowerSearch));
            }
        }
    }
}
