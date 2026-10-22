using Gymify.Application.Common.CQRS;
using Gymify.Application.Security;
using Gymify.Application.Security.Markers;
using Gymify.Domain.Common.Enums;

namespace Gymify.Application.Features.Customers.Commands.ChangeCustomerApprovalStatus
{
    [Authorize(Permissions = [Permission.CustomersWrite])]
    public class ChangeCustomerApprovalStatusCommand : ICommand<ChangeCustomerApprovalStatusCommandResponse>, IAuthorizedRequest
    {
        public int CustomerId { get; set; }
        public ApprovalStatus Status { get; set; }

        public ChangeCustomerApprovalStatusCommand()
        {
        }

        public ChangeCustomerApprovalStatusCommand(int customerId, ApprovalStatus status)
        {
            CustomerId = customerId;
            Status = status;
        }
    }
}
