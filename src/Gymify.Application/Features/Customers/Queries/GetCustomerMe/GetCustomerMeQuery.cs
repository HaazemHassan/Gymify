using Gymify.Application.Security;
using Gymify.Application.Security.Markers;

namespace Gymify.Application.Features.Customers.Queries.GetCustomerMe
{
    [Authorize]
    public class GetCustomerMeQuery : IQuery<GetCustomerMeQueryResponse>, IAuthorizedRequest
    {
    }
}
