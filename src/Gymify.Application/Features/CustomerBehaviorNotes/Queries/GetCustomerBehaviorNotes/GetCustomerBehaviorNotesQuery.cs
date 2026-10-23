using Gymify.Application.Common.Pagination;
using Gymify.Application.Security;
using Gymify.Application.Security.Markers;
using Gymify.Application.Security.Policies;
using Gymify.Domain.Common.Enums;

namespace Gymify.Application.Features.CustomerBehaviorNotes.Queries.GetCustomerBehaviorNotes
{
    [Authorize(Permissions = [Permission.CustomersRead])]
    [Authorize(Policy = AuthorizationPolicies.SelfCustomerOnly)]
    public class GetCustomerBehaviorNotesQuery : PaginatedQuery, IQuery<PaginatedResult<GetCustomerBehaviorNotesQueryResponse>>, ICustomerResourceRequest
    {
        public int CustomerId { get; set; }
    }
}
