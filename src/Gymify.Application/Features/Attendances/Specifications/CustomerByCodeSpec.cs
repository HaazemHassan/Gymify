using Ardalis.Specification;
using Gymify.Domain.Entities;

namespace Gymify.Application.Features.Attendances.Specifications
{
    public class CustomerByCodeSpec : Specification<Customer>, ISingleResultSpecification<Customer>
    {
        public CustomerByCodeSpec(string code)
        {
            Query.Where(c => c.Code == code)
                .Include(c => c.DomainUser);
        }
    }
}
