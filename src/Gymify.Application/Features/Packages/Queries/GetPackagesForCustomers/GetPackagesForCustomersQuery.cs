using Gymify.Application.Common.CQRS;
using Gymify.Application.Common.Pagination;
using Gymify.Domain.Common.Enums;

namespace Gymify.Application.Features.Packages.Queries.GetPackagesForCustomers
{
    public class GetPackagesForCustomersQuery : PaginatedQuery, IQuery<PaginatedResult<GetPackagesForCustomersQueryResponse>>
    {
        public List<PackageType>? Types { get; set; }
    }
}
