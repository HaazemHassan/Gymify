using AutoMapper;
using Gymify.Domain.Entities;
using Gymify.Domain.RepositoriesContracts;

namespace Gymify.Infrastructure.Data.Repositories
{
    internal sealed class LockerRepository(AppDbContext context, IMapper mapper)
        : GenericRepository<Locker>(context, mapper), ILockerRepository
    {
    }
}
