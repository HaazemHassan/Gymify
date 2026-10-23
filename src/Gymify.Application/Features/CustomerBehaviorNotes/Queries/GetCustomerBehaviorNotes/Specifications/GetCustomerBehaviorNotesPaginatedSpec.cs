using Ardalis.Specification;
using Gymify.Application.Specifications;
using Gymify.Domain.Entities;

namespace Gymify.Application.Features.CustomerBehaviorNotes.Queries.GetCustomerBehaviorNotes.Specifications
{
    public class GetCustomerBehaviorNotesPaginatedSpec : Specification<CustomerBehaviorNote>
    {
        public GetCustomerBehaviorNotesPaginatedSpec(int pageNumber, int pageSize, int customerId, string? search = null)
        {
            Query.Where(n => n.CustomerId == customerId);

            if (!string.IsNullOrWhiteSpace(search))
            {
                var term = search.Trim();
                Query.Where(n => n.Description.Contains(term) ||
                                 n.Customer.DomainUser.Name.Contains(term) ||
                                 n.Customer.DomainUser.Email.Contains(term) ||
                                 (n.Customer.Code != null && n.Customer.Code.Contains(term)) ||
                                 n.Employee.DomainUser.Name.Contains(term));
            }

            Query.OrderByDescending(n => n.CreatedAt);
            Query.Paginate(pageNumber, pageSize);
        }
    }
}
