using Ardalis.Specification;
using Gymify.Domain.Entities;

namespace Gymify.Application.Features.Customers.Commands.UpdateCustomer.Specifications
{
    public class CustomerWithDomainUserByIdSpec : Specification<Customer>
    {
        public CustomerWithDomainUserByIdSpec(int customerId)
        {
            Query
                .Where(c => c.Id == customerId)
                .Include(c => c.DomainUser);
        }
    }
}
