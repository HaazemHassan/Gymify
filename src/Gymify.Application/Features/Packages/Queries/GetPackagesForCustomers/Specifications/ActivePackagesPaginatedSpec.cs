using Ardalis.Specification;
using Gymify.Application.Features.Packages.Specifications;
using Gymify.Application.Specifications;

namespace Gymify.Application.Features.Packages.Queries.GetPackagesForCustomers.Specifications
{
    public class ActivePackagesPaginatedSpec : PackagesFilteredSpec
    {
        public ActivePackagesPaginatedSpec(int pageNumber, int pageSize, string? search = null)
            : base(search, isActive: true)
        {
            Query.OrderBy(p => p.Price);
            Query.Paginate(pageNumber, pageSize);
            Query.AsNoTracking();
        }
    }
}
