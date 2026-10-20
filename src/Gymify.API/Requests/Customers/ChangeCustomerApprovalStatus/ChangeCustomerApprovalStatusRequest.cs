using Gymify.Domain.Common.Enums;

namespace Gymify.API.Requests.Customers.ChangeCustomerApprovalStatus
{
    public class ChangeCustomerApprovalStatusRequest
    {
        public ApprovalStatus Status { get; set; }
    }
}
