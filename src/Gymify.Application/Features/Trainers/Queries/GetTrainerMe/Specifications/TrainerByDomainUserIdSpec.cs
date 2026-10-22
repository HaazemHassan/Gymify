using Ardalis.Specification;
using Gymify.Domain.Entities;

namespace Gymify.Application.Features.Trainers.Queries.GetTrainerMe.Specifications
{
    public class TrainerByDomainUserIdSpec : Specification<Trainer>
    {
        public TrainerByDomainUserIdSpec(int domainUserId)
        {
            Query
                .Where(t => t.Employee.DomainUserId == domainUserId)
                .Include(t => t.Employee)
                    .ThenInclude(e => e.DomainUser);
        }
    }
}
