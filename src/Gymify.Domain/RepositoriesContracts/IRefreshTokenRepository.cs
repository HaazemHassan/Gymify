using Gymify.Domain.Entities;

namespace Gymify.Domain.Repositories
{
    public interface IRefreshTokenRepository : IGenericRepository<RefreshToken>
    {
        Task DeleteExpiredTokensAsync(DateTimeOffset cutoffDate);
    }
}
