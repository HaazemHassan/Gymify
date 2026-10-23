using AutoMapper;
using Gymify.Domain.Entities;
using Gymify.Domain.RepositoriesContracts;

namespace Gymify.Infrastructure.Data.Repositories
{
    internal sealed class KpiRepository(AppDbContext context, IMapper mapper)
        : GenericRepository<Kpi>(context, mapper), IKpiRepository
    {
    }
}
