using Gymify.Application.Security;
using Gymify.Application.Security.Markers;

namespace Gymify.Application.Features.Authentication.Commands.Logout
{
    [Authorize]
    public class LogoutCommand : ICommand, IAuthorizedRequest
    {
        public string? RefreshToken { get; set; }
    }
}
