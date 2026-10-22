using Ardalis.Specification;
using Gymify.Domain.Common.Enums;
using Gymify.Domain.Entities;

namespace Gymify.Application.Features.Packages.Specifications
{
    public class PackagesFilteredSpec : Specification<Package>
    {
        public PackagesFilteredSpec(
            string? search = null,
            bool? isActive = null,
            IEnumerable<PackageType>? types = null,
            int? trainerId = null)
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

            if (types is not null && types.Any())
            {
                var typeList = types.Distinct().ToList();
                Query.Where(p => typeList.Contains(p.Type));
            }

            if (trainerId.HasValue)
            {
                Query.Where(p => p.TrainerId == trainerId.Value);
            }
        }
    }
}
