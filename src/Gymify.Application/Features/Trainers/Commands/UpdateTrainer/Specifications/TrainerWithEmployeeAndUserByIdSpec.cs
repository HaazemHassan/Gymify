using Ardalis.Specification;
using Gymify.Domain.Entities;

namespace Gymify.Application.Features.Trainers.Commands.UpdateTrainer.Specifications
{
    public class TrainerWithEmployeeAndUserByIdSpec : Specification<Trainer>
    {
        public TrainerWithEmployeeAndUserByIdSpec(int trainerId)
        {
            Query
                .Where(t => t.Id == trainerId)
                .Include(t => t.Employee)
                    .ThenInclude(e => e.DomainUser);
        }
    }
}
