using Gymify.Application.Features.Authentication.Common;

namespace Gymify.Application.ServicesContracts.Infrastructure
{
    public interface IGoogleAuthService
    {
        Task<ErrorOr<GoogleUserInfo>> ValidateIdTokenAsync(string idToken, CancellationToken ct = default);

    }
}
