using Gymify.Domain.Common.Enums;

namespace Gymify.Application.Features.Customers.Commands.ChangeCustomerApprovalStatus
{
    public class ChangeCustomerApprovalStatusCommandResponse
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int UserId { get; set; }
        public ApprovalStatus ApprovalStatus { get; set; }
    }
}
