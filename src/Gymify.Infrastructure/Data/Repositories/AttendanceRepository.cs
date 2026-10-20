using AutoMapper;
using Gymify.Domain.Entities;
using Gymify.Domain.RepositoriesContracts;

namespace Gymify.Infrastructure.Data.Repositories
{
    internal sealed class AttendanceRepository(AppDbContext context, IMapper mapper)
        : GenericRepository<Attendance>(context, mapper), IAttendanceRepository
    {
    }
}
