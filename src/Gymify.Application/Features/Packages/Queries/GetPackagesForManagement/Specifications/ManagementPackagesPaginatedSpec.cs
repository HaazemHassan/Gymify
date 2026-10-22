using Ardalis.Specification;
using Gymify.Application.Features.Packages.Specifications;
using Gymify.Application.Specifications;
using Gymify.Domain.Common.Enums;

namespace Gymify.Application.Features.Packages.Queries.GetPackagesForManagement.Specifications
{
    public class ManagementPackagesPaginatedSpec : PackagesFilteredSpec
    {
        public ManagementPackagesPaginatedSpec(
            int pageNumber,
            int pageSize,
            string? search = null,
            IEnumerable<PackageType>? types = null,
            int? trainerId = null,
            bool? isActive = null)
            : base(search, isActive: isActive, types: types, trainerId: trainerId)
        {
            Query.OrderByDescending(p => p.CreatedAt);
            Query.Paginate(pageNumber, pageSize);
            Query.AsNoTracking();
        }
    }
}
