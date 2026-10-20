namespace Gymify.Application.Features.Authentication.Common
{
    public record GoogleUserInfo(
     string GoogleId,
     string Email,
     string Name,
     bool IsEmailVerified
 );
}
