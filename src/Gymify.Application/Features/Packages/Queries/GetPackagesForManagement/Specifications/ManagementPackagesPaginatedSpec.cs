using Ardalis.Specification;
using Gymify.Application.Features.Packages.Specifications;
using Gymify.Application.Specifications;

namespace Gymify.Application.Features.Packages.Queries.GetPackagesForManagement.Specifications
{
    public class ManagementPackagesPaginatedSpec : PackagesFilteredSpec
    {
        public ManagementPackagesPaginatedSpec(int pageNumber, int pageSize, string? search = null)
            : base(search, isActive: null)
        {
            Query.OrderByDescending(p => p.CreatedAt);
            Query.Paginate(pageNumber, pageSize);
            Query.AsNoTracking();
        }
    }
}
