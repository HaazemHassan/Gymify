using AutoMapper;
using Gymify.Domain.Entities;
using Gymify.Domain.RepositoriesContracts;
using Microsoft.EntityFrameworkCore;

namespace Gymify.Infrastructure.Data.Repositories
{
    internal sealed class SubscriptionRepository(AppDbContext context, IMapper mapper)
        : GenericRepository<Subscription>(context, mapper), ISubscriptionRepository
    {
        private readonly AppDbContext _context = context;

        public async Task<List<PaymentMethodRevenueAggregate>> GetPaymentMethodsRevenueSummaryAsync(
            DateTimeOffset? fromDate,
            DateTimeOffset? toDate,
            CancellationToken ct = default)
        {
            var query = _context.Subscriptions.AsNoTracking();

            if (fromDate.HasValue)
            {
                query = query.Where(s => s.CreatedAt >= fromDate.Value);
            }

            if (toDate.HasValue)
            {
                query = query.Where(s => s.CreatedAt <= toDate.Value);
            }

            return await query
                .GroupBy(s => s.PaymentMethod)
                .Select(g => new PaymentMethodRevenueAggregate(
                    g.Key,
                    g.Sum(s => s.PackagePrice),
                    g.Count()
                ))
                .ToListAsync(ct);
        }
    }
}

