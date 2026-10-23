using Gymify.Application.Common.CQRS;
using Gymify.Application.Security;
using Gymify.Application.Security.Markers;
using Gymify.Domain.Common.Enums;

namespace Gymify.Application.Features.CustomerBehaviorNotes.Commands.CreateCustomerBehaviorNote
{
    [Authorize(Permissions = [Permission.CustomerBehaviorWrite])]
    [Authorize(Permissions = [Permission.CustomersWrite])]
    public class CreateCustomerBehaviorNoteCommand : ICommand<CreateCustomerBehaviorNoteCommandResponse>, IAuthorizedRequest
    {
        public int CustomerId { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}
