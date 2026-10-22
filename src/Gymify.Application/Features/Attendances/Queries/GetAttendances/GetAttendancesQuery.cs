using Gymify.Application.Common.CQRS;
using Gymify.Application.Common.Pagination;
using Gymify.Application.Security;
using Gymify.Application.Security.Markers;
using Gymify.Domain.Common.Enums;

namespace Gymify.Application.Features.Attendances.Queries.GetAttendances
{
    [Authorize(Permissions = [Permission.AttendanceRead])]
    public class GetAttendancesQuery : PaginatedQuery, IQuery<PaginatedResult<GetAttendancesQueryResponse>>, IAuthorizedRequest
    {
        public bool? IsCurrentlyAttending { get; set; }
        public DateTimeOffset? FromDate { get; set; }
    }
}
