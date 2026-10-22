using Gymify.Application.Common.CQRS;
using Gymify.Application.Security;
using Gymify.Application.Security.Markers;
using Gymify.Domain.Common.Enums;

namespace Gymify.Application.Features.Customers.Queries.GetCustomerById
{
    [Authorize(Permissions = [Permission.CustomersRead])]
    public class GetCustomerByIdQuery : IQuery<GetCustomerByIdQueryResponse>, IAuthorizedRequest
    {
        public int Id { get; set; }

        public GetCustomerByIdQuery()
        {
        }

        public GetCustomerByIdQuery(int id)
        {
            Id = id;
        }
    }
}
