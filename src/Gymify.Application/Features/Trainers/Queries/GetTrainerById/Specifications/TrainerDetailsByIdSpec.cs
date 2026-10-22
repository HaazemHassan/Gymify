using Ardalis.Specification;
using Gymify.Domain.Entities;

namespace Gymify.Application.Features.Trainers.Queries.GetTrainerById.Specifications
{
    public class TrainerDetailsByIdSpec : Specification<Trainer>
    {
        public TrainerDetailsByIdSpec(int trainerId)
        {
            Query
                .Where(t => t.Id == trainerId)
                .Include(t => t.Employee)
                    .ThenInclude(e => e.DomainUser)
                .AsNoTracking();
        }
    }
}
