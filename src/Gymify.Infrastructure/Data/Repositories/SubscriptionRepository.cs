using AutoMapper;
using Gymify.Domain.Entities;
using Gymify.Domain.RepositoriesContracts;

namespace Gymify.Infrastructure.Data.Repositories
{
    internal sealed class SubscriptionRepository(AppDbContext context, IMapper mapper)
        : GenericRepository<Subscription>(context, mapper), ISubscriptionRepository
    {
    }
}
