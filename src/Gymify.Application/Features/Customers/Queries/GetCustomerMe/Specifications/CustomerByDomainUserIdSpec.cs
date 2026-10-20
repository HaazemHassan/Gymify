using Ardalis.Specification;
using Gymify.Domain.Entities;

namespace Gymify.Application.Features.Customers.Queries.GetCustomerMe.Specifications
{
    public class CustomerByDomainUserIdSpec : Specification<Customer>
    {
        public CustomerByDomainUserIdSpec(int domainUserId)
        {
            Query
                .Where(c => c.DomainUserId == domainUserId)
                .Include(c => c.DomainUser);
        }
    }
}
