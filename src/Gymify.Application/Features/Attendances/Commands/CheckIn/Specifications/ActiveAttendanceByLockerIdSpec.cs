using Ardalis.Specification;
using Gymify.Domain.Entities;

namespace Gymify.Application.Features.Attendances.Commands.CheckIn.Specifications
{
    public class ActiveAttendanceByLockerIdSpec : Specification<Attendance>, ISingleResultSpecification<Attendance>
    {
        public ActiveAttendanceByLockerIdSpec(int lockerId)
        {
            Query.Where(a => a.LockerId == lockerId && a.CheckOutAt == null);
        }
    }
}
