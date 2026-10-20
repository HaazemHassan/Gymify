using Ardalis.Specification;
using Gymify.Domain.Entities;

namespace Gymify.Application.Features.Lockers.Commands.CreateLocker.Specifications
{
    public class LockerByNumberSpec : Specification<Locker>, ISingleResultSpecification<Locker>
    {
        public LockerByNumberSpec(int number)
        {
            Query
                .Where(l => l.Number == number);
        }
    }
}
