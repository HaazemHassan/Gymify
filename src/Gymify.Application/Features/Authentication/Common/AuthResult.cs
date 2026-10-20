using System.Text.Json.Serialization;

namespace Gymify.Application.Features.Authentication.Common
{
    public class AuthResult(string accessToken, RefreshTokenDTO refreshToken, int domainUserId)
    {
        public string AccessToken { get; set; } = accessToken;

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public RefreshTokenDTO? RefreshToken { get; set; } = refreshToken;

        [JsonIgnore]
        public int DomainUserId { get; set; } = domainUserId;
    }


    public class RefreshTokenDTO
    {
        public string Token { get; set; } = string.Empty;
        public int UserId { get; set; }
        public DateTimeOffset ExpirationDate { get; set; }
    }
}
