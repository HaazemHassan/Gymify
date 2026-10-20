using Ardalis.Specification;
using Gymify.Domain.Entities;

namespace Gymify.Domain.Specifications.RefreshTokens
{
    public class ActiveRefreshTokenByJtiAndTokenSpec : Specification<RefreshToken>
    {
        public ActiveRefreshTokenByJtiAndTokenSpec(string accessTokenJti, string refreshToken, int userId)
        {
            Query.Where(x =>
                    x.AccessTokenJTI == accessTokenJti &&
                    x.Token == refreshToken &&
                    x.UserId == userId &&
                    x.RevokationDate == null &&
                    x.Expires > DateTimeOffset.UtcNow
            );
        }
    }
}
