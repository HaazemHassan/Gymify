using Ardalis.Specification;
using Gymify.Domain.Entities;

namespace Gymify.Application.Features.Attendances.Commands.CheckOut.Specifications
{
    public class ActiveAttendanceForCustomerSpec : Specification<Attendance>, ISingleResultSpecification<Attendance>
    {
        public ActiveAttendanceForCustomerSpec(int customerId)
        {
            Query
                .Where(a => a.CustomerId == customerId && a.CheckOutAt == null);
        }
    }
}
