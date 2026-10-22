using Gymify.Application.Common.CQRS;
using Gymify.Application.Security;
using Gymify.Application.Security.Markers;
using Gymify.Application.Security.Policies;
using Gymify.Domain.Common.Enums;

namespace Gymify.Application.Features.Attendances.Queries.GetCustomerAttendances
{
    [Authorize(Permissions = [Permission.AttendanceRead])]
    [Authorize(Policy = AuthorizationPolicies.SelfCustomerOnly)]
    public class GetCustomerAttendancesQuery : IQuery<GetCustomerAttendancesQueryResponse>, ICustomerResourceRequest
    {
        public int CustomerId { get; set; }
        public int? Month { get; set; }
        public int? Year { get; set; }

        public GetCustomerAttendancesQuery()
        {
        }

        public GetCustomerAttendancesQuery(int customerId, int? month = null, int? year = null)
        {
            CustomerId = customerId;
            Month = month;
            Year = year;
        }
    }
}
