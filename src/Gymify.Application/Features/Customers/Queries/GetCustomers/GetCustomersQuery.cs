using Gymify.Application.Common.CQRS;
using Gymify.Application.Common.Pagination;
using Gymify.Application.Security;
using Gymify.Application.Security.Markers;
using Gymify.Domain.Common.Enums;

namespace Gymify.Application.Features.Customers.Queries.GetCustomers
{
    [Authorize(Permissions = [Permission.UsersRead])]
    public class GetCustomersQuery : PaginatedQuery, IQuery<PaginatedResult<GetCustomersQueryResponse>>, IAuthorizedRequest
    {
        public ApprovalStatus? ApprovalStatus { get; set; }
    }
}
