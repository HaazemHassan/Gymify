using AutoMapper;
using Gymify.Domain.Entities;
using Gymify.Domain.RepositoriesContracts;

namespace Gymify.Infrastructure.Data.Repositories
{
    internal sealed class CustomerRepository(AppDbContext context, IMapper mapper)
        : GenericRepository<Customer>(context, mapper), ICustomerRepository
    {
    }
}
