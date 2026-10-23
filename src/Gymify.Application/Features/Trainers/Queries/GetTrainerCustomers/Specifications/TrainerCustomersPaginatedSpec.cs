using Ardalis.Specification;
using Gymify.Application.Specifications;
using Gymify.Domain.Entities;

namespace Gymify.Application.Features.Trainers.Queries.GetTrainerCustomers.Specifications
{
    public class TrainerCustomersPaginatedSpec : Specification<Subscription>
    {
        public TrainerCustomersPaginatedSpec(int trainerId, DateTimeOffset now, int pageNumber, int pageSize, string? search = null)
        {
            Query.Where(s => s.TrainerId == trainerId && s.StartDate <= now && s.EndDate >= now);

            if (!string.IsNullOrWhiteSpace(search))
            {
                var term = search.Trim();
                Query.Where(s => (s.Customer.DomainUser.Name.Contains(term)) ||
                                 (s.Customer.DomainUser.Email.Contains(term)) ||
                                 (s.Customer.DomainUser.PhoneNumber != null && s.Customer.DomainUser.PhoneNumber.Contains(term)) ||
                                 (s.Customer.Code != null && s.Customer.Code.Contains(term)));
            }

            Query.OrderByDescending(s => s.CreatedAt);
            Query.Paginate(pageNumber, pageSize);
        }
    }
}
