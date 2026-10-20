using Ardalis.Specification;
using Gymify.Domain.Entities;

namespace Gymify.Application.Features.Attendances.Commands.CheckIn.Specifications
{
    public class LastCheckInByCustomerIdSpec : Specification<Attendance>, ISingleResultSpecification<Attendance>
    {
        public LastCheckInByCustomerIdSpec(int customerId)
        {
            Query.Where(a => a.CustomerId == customerId)
                .OrderByDescending(a => a.CheckInAt);
        }
    }
}
