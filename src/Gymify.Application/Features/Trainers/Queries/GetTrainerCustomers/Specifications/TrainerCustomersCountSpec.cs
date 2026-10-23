using Ardalis.Specification;
using Gymify.Domain.Entities;

namespace Gymify.Application.Features.Trainers.Queries.GetTrainerCustomers.Specifications
{
    public class TrainerCustomersCountSpec : Specification<Subscription>
    {
        public TrainerCustomersCountSpec(int trainerId, DateTimeOffset now, string? search = null)
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
        }
    }
}
