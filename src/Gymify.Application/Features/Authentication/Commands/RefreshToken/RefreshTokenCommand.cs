using Gymify.Application.Features.Authentication.Common;

namespace Gymify.Application.Features.Authentication.Commands.RefreshToken;

public class RefreshTokenCommand : ICommand<AuthResponse>
{
    public string AccessToken { get; set; } = string.Empty;
    public string? RefreshToken { get; set; }
}
