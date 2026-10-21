using Ardalis.Specification;
using Gymify.Domain.Entities;

namespace Gymify.Application.Features.Customers.Queries.GetCustomerById.Specifications
{
    public class CustomerDetailsByIdSpec : Specification<Customer>
    {
        public CustomerDetailsByIdSpec(int customerId)
        {
            Query
                .Where(c => c.Id == customerId)
                .Include(c => c.DomainUser)
                .AsNoTracking();
        }
    }
}
