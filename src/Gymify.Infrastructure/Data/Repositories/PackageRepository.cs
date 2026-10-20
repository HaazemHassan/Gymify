using AutoMapper;
using Gymify.Domain.Entities;
using Gymify.Domain.RepositoriesContracts;

namespace Gymify.Infrastructure.Data.Repositories
{
    internal sealed class PackageRepository(AppDbContext context, IMapper mapper)
        : GenericRepository<Package>(context, mapper), IPackageRepository
    {
    }
}
