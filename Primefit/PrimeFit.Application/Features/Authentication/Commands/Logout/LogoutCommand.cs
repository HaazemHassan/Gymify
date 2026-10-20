using PrimeFit.Application.Security;
using PrimeFit.Application.Security.Markers;

namespace PrimeFit.Application.Features.Authentication.Commands.Logout
{
    [Authorize]
    public class LogoutCommand : ICommand, IAuthorizedRequest
    {
        public string? RefreshToken { get; set; }
    }
}
