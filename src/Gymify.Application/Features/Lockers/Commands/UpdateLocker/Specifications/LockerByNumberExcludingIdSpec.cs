using Ardalis.Specification;
using Gymify.Domain.Entities;

namespace Gymify.Application.Features.Lockers.Commands.UpdateLocker.Specifications
{
    public class LockerByNumberExcludingIdSpec : Specification<Locker>, ISingleResultSpecification<Locker>
    {
        public LockerByNumberExcludingIdSpec(int number, int excludeId)
        {
            Query
                .Where(l => l.Number == number && l.Id != excludeId);
        }
    }
}
