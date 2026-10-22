using Ardalis.Specification;
using Gymify.Application.Features.Packages.Specifications;
using Gymify.Application.Specifications;
using Gymify.Domain.Common.Enums;

namespace Gymify.Application.Features.Packages.Queries.GetPackagesForCustomers.Specifications
{
    public class ActivePackagesPaginatedSpec : PackagesFilteredSpec
    {
        public ActivePackagesPaginatedSpec(int pageNumber, int pageSize, string? search = null, IEnumerable<PackageType>? types = null)
            : base(search, isActive: true, types: types)
        {
            Query.OrderBy(p => p.Price);
            Query.Paginate(pageNumber, pageSize);
            Query.AsNoTracking();
        }
    }
}
