using AutoMapper;
using Gymify.Domain.Entities;
using Gymify.Domain.RepositoriesContracts;

namespace Gymify.Infrastructure.Data.Repositories
{
    internal sealed class CustomerBehaviorNoteRepository(AppDbContext context, IMapper mapper)
        : GenericRepository<CustomerBehaviorNote>(context, mapper), ICustomerBehaviorNoteRepository
    {
    }
}
