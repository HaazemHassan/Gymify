using PrimeFit.Application.Features.Authentication.Common;

namespace PrimeFit.Application.Features.Authentication.Commands.RefreshToken;

public class RefreshTokenCommand : ICommand<AuthResult>
{
    public string AccessToken { get; set; } = string.Empty;
    public string? RefreshToken { get; set; }

}
