using Gymify.Application.Common.Transaction;
using Gymify.Application.Security.Markers;
using Gymify.Domain.Common.Enums;

namespace Gymify.Application.Features.Users.Commands.AddUser
{
    //[Authorize(Permissions = [Permission.UsersWrite])]
    public class AddUserCommand : ICommand<AddUserCommandResponse>, ITransactionalRequest, IAuthorizedRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
        public UserRole UserRole { get; set; }

    }
}
