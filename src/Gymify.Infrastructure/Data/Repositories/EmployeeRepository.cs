using AutoMapper;
using Gymify.Domain.Entities;
using Gymify.Domain.RepositoriesContracts;

namespace Gymify.Infrastructure.Data.Repositories
{
    internal sealed class EmployeeRepository(AppDbContext context, IMapper mapper)
        : GenericRepository<Employee>(context, mapper), IEmployeeRepository
    {
    }
}
