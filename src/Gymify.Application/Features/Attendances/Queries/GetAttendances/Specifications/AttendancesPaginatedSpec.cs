using Ardalis.Specification;
using Gymify.Application.Features.Attendances.Specifications;
using Gymify.Application.Specifications;

namespace Gymify.Application.Features.Attendances.Queries.GetAttendances.Specifications
{
    public class AttendancesPaginatedSpec : AttendancesFilteredSpec
    {
        public AttendancesPaginatedSpec(
            int pageNumber,
            int pageSize,
            string? search = null,
            bool? isCurrentlyAttending = null,
            DateTimeOffset? fromDate = null)
            : base(search, isCurrentlyAttending, fromDate)
        {
            Query.OrderByDescending(a => a.CheckInAt);
            Query.Paginate(pageNumber, pageSize);
            Query.AsNoTracking();
        }
    }
}
