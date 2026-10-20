using Gymify.Application.Common.CQRS;
using Gymify.Application.Common.Pagination;

namespace Gymify.Application.Features.Packages.Queries.GetPackagesForCustomers
{
    public class GetPackagesForCustomersQuery : PaginatedQuery, IQuery<PaginatedResult<GetPackagesForCustomersQueryResponse>>
    {
    }
}
