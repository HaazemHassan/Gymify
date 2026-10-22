using Ardalis.Specification;
using Gymify.Application.Common.Helpers;
using Gymify.Domain.Entities;

namespace Gymify.Application.Features.Attendances.Queries.GetCustomerAttendances.Specifications
{
    public class CustomerAttendancesSpec : Specification<Attendance>
    {
        public CustomerAttendancesSpec(int customerId, int month, int year)
        {
            Query.Where(a => a.CustomerId == customerId);

            var (start, end) = DateTimeHelper.GetMonthDateRange(year, month);
            Query.Where(a => a.CheckInAt >= start && a.CheckInAt < end);

            Query.OrderByDescending(a => a.CheckInAt);
            Query.AsNoTracking();
        }
    }
}
