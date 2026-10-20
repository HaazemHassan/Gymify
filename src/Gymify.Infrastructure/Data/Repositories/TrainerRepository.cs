using AutoMapper;
using Gymify.Domain.Entities;
using Gymify.Domain.RepositoriesContracts;

namespace Gymify.Infrastructure.Data.Repositories
{
    internal sealed class TrainerRepository(AppDbContext context, IMapper mapper)
        : GenericRepository<Trainer>(context, mapper), ITrainerRepository
    {
    }
}
