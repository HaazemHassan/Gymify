using Ardalis.Specification;
using Gymify.Domain.Entities;

namespace Gymify.Application.Features.Lockers.Queries.GetLockers.Specifications
{
    public class GetLockersSpec : Specification<Locker>
    {
        public GetLockersSpec(bool? isActive)
        {
            Query.OrderBy(l => l.Number);

            if (isActive.HasValue)
            {
                Query.Where(l => l.IsActive == isActive.Value);
            }
        }
    }
}
