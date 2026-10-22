using Gymify.Application.Common.CQRS;
using Gymify.Application.Security;
using Gymify.Application.Security.Markers;
using Gymify.Application.Security.Policies;
using Gymify.Domain.Common.Enums;

namespace Gymify.Application.Features.Customers.Queries.GetCustomerById
{
    [Authorize(Permissions = [Permission.CustomersRead])]
    [Authorize(Policy = AuthorizationPolicies.SelfCustomerOnly)]
    public class GetCustomerByIdQuery : IQuery<GetCustomerByIdQueryResponse>, ICustomerResourceRequest
    {
        public int Id { get; set; }

        public int CustomerId => Id;

        public GetCustomerByIdQuery()
        {
        }

        public GetCustomerByIdQuery(int id)
        {
            Id = id;
        }
    }
}
