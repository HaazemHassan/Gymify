using Gymify.Application.Security;
using Gymify.Application.Security.Markers;

namespace Gymify.Application.Features.Authentication.Commands.ChangePassword
{

    [Authorize]
    public class ChangePasswordCommand : ICommand, IAuthorizedRequest
    {
        public string CurrentPassword { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
        public string ConfirmNewPassword { get; set; } = string.Empty;
    }
}
