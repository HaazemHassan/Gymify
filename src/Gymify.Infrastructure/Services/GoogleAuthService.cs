using ErrorOr;
using Google.Apis.Auth;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Gymify.Application.Features.Authentication.Common;
using Gymify.Application.ServicesContracts.Infrastructure;
using Gymify.Domain.Common.Constants;
using Gymify.Infrastructure.Common.Options;

namespace Gymify.Infrastructure.Services
{
    internal sealed class GoogleAuthService(IOptions<GoogleAuthOptions> googleOptions, ILogger<GoogleAuthService> logger) : IGoogleAuthService
    {
        private readonly GoogleAuthOptions _googleOptions = googleOptions.Value;

        public async Task<ErrorOr<GoogleUserInfo>> ValidateIdTokenAsync(string idToken, CancellationToken ct = default)
        {
            try
            {
                var settings = new GoogleJsonWebSignature.ValidationSettings
                {
                    Audience = [_googleOptions.ClientId],
                    ExpirationTimeClockTolerance = TimeSpan.FromMinutes(1),
                    IssuedAtClockTolerance = TimeSpan.FromMinutes(1)
                };

                var payload = await GoogleJsonWebSignature.ValidateAsync(idToken, settings);

                if (!payload.EmailVerified)
                {
                    logger.LogWarning(
                        "Google login attempt with unverified email: {Email}",
                        payload.Email);

                    return Error.Validation(
                        code: ErrorCodes.Authentication.InvalidCredentials,
                        description: "Google account email is not verified.");
                }

                var name =
                    payload.Name ??
                    $"{payload.GivenName} {payload.FamilyName}".Trim();
                if (string.IsNullOrWhiteSpace(name))
                    name = "User";

                return new GoogleUserInfo(
                    payload.Subject,
                    payload.Email,
                    name,
                    payload.EmailVerified
                );
            }
            catch (InvalidJwtException ex)
            {
                logger.LogWarning(
                    ex,
                    "Invalid or expired Google ID token received");



                return Error.Validation(
                    code: ErrorCodes.Authentication.InvalidAccessToken,
                    description: "Invalid ID token.");
            }
        }
    }
}


