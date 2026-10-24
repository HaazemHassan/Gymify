using Ardalis.Specification;
using Gymify.Domain.Entities;

namespace Gymify.Application.Features.Attendances.Specifications
{
    public class ActiveCustomerByCodeSpec : Specification<Customer>, ISingleResultSpecification<Customer>
    {
        public ActiveCustomerByCodeSpec(string code)
        {
            Query.Where(c => c.Code == code)
                .Include(c => c.DomainUser).Where(c => c.DomainUser.IsActive);
        }
    }
}
