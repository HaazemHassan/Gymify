using Ardalis.Specification;
using Gymify.Domain.Common.Enums;
using Gymify.Domain.Entities;

namespace Gymify.Application.Features.Customers.Queries.GetCustomers.Specifications
{
    public class CustomersCountSpec : Specification<Customer>
    {
        public CustomersCountSpec(
            string? search = null,
            ApprovalStatus? approvalStatus = null)
        {
            if (!string.IsNullOrWhiteSpace(search))
            {
                var term = search.Trim();
                Query.Where(c => (c.DomainUser != null && c.DomainUser.Name.Contains(term)) ||
                                 (c.DomainUser != null && c.DomainUser.Email.Contains(term)) ||
                                 (c.DomainUser != null && c.DomainUser.PhoneNumber != null && c.DomainUser.PhoneNumber.Contains(term)) ||
                                 (c.Code != null && c.Code.Contains(term)));
            }

            if (approvalStatus.HasValue)
            {
                Query.Where(c => c.ApprovalStatus == approvalStatus.Value);
            }
        }
    }
}
