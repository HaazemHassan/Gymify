using Gymify.Api.Requests.Common.Pagination;
using Gymify.Domain.Common.Enums;

namespace Gymify.API.Requests.Customers.GetCustomers
{
    public class GetCustomersRequest : BasicPaginationRequest
    {
        public ApprovalStatus? ApprovalStatus { get; set; }
    }
}
