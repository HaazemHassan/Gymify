using Ardalis.Specification;
using Gymify.Domain.Entities;

namespace Gymify.Application.Features.Attendances.Specifications
{
    public class AttendancesFilteredSpec : Specification<Attendance>
    {
        public AttendancesFilteredSpec(
            string? search = null,
            bool? isCurrentlyAttending = null,
            DateTimeOffset? fromDate = null)
        {
            if (isCurrentlyAttending.HasValue)
            {
                if (isCurrentlyAttending.Value)
                {
                    Query.Where(a => a.CheckOutAt == null);
                }
                else
                {
                    Query.Where(a => a.CheckOutAt != null);
                }
            }

            if (fromDate.HasValue)
            {
                Query.Where(a => a.CheckInAt >= fromDate.Value);
            }

            if (!string.IsNullOrWhiteSpace(search))
            {
                var trimmed = search.Trim();
                Query.Where(a =>
                    a.Customer.DomainUser.Name.Contains(trimmed) ||
                    a.Customer.DomainUser.Email.Contains(trimmed) ||
                    (a.Customer.DomainUser.PhoneNumber != null && a.Customer.DomainUser.PhoneNumber.Contains(trimmed)) ||
                    (a.Code != null && a.Code.Contains(trimmed)));
            }
        }
    }
}
