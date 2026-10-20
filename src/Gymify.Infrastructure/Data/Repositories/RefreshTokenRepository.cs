using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Gymify.Domain.Entities;
using Gymify.Domain.Repositories;

namespace Gymify.Infrastructure.Data.Repositories
{
    internal class RefreshTokenRepository : GenericRepository<RefreshToken>, IRefreshTokenRepository
    {

        private readonly DbSet<RefreshToken> _refreshTokens;


        public RefreshTokenRepository(AppDbContext context, IMapper mapper) : base(context, mapper)
        {
            _refreshTokens = context.Set<RefreshToken>();
        }

        public async Task DeleteExpiredTokensAsync(DateTimeOffset cutoffDate)
        {

            await _refreshTokens
                .Where(x => (x.RevokationDate != null && x.RevokationDate <= cutoffDate) ||
                            (x.Expires <= cutoffDate))
                .ExecuteDeleteAsync();
        }
    }
}
