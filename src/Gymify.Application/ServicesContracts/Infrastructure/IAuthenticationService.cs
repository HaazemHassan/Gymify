using Gymify.Application.Features.Authentication.Common;
using Gymify.Domain.Common.Enums;

namespace Gymify.Application.ServicesContracts.Infrastructure
{
    public interface IAuthenticationService
    {
        public Task<ErrorOr<AuthResult>> SignInWithPasswordAsync(string Email, string password, UserType requestedUserType, CancellationToken ct = default);
        public Task<ErrorOr<AuthResult>> ReAuthenticateAsync(string refreshToken, string accessToken, CancellationToken ct = default);
        public Task<ErrorOr<Success>> LogoutAsync(string refreshToken, CancellationToken ct = default);
        Task<ErrorOr<AuthResult>> SignInWithGoogleAsync(GoogleUserInfo googleUser, Gymify.Domain.Common.Enums.UserType requestedUserType, CancellationToken ct = default);
    }
}
