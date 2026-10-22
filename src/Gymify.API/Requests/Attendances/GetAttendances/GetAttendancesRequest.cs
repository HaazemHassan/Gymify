using Gymify.Api.Requests.Common.Pagination;

namespace Gymify.API.Requests.Attendances.GetAttendances
{
    public class GetAttendancesRequest : BasicPaginationRequest
    {
        public bool? IsCurrentlyAttending { get; set; }
        public DateTimeOffset? FromDate { get; set; }
    }
}
