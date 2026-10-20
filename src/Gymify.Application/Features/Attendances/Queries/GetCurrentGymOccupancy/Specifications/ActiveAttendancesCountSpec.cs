using Ardalis.Specification;
using Gymify.Domain.Entities;

namespace Gymify.Application.Features.Attendances.Queries.GetCurrentGymOccupancy.Specifications
{
    public class ActiveAttendancesCountSpec : Specification<Attendance>
    {
        public ActiveAttendancesCountSpec()
        {
            Query
                .Where(a => a.CheckOutAt == null);
        }
    }
}
