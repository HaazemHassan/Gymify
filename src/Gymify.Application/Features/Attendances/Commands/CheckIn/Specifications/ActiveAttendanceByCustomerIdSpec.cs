using Ardalis.Specification;
using Gymify.Domain.Entities;

namespace Gymify.Application.Features.Attendances.Commands.CheckIn.Specifications
{
    public class ActiveAttendanceByCustomerIdSpec : Specification<Attendance>, ISingleResultSpecification<Attendance>
    {
        public ActiveAttendanceByCustomerIdSpec(int customerId)
        {
            Query.Where(a => a.CustomerId == customerId && a.CheckOutAt == null);
        }
    }
}
