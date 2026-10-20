using ErrorOr;

namespace Gymify.Api.Requests.Authentication.Logout;

public class LogoutRequest
{
        public string? RefreshToken { get; set; }
}
