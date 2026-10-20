
using Ardalis.Specification;
using Gymify.Domain.Entities.Base;

namespace Gymify.Application.Specifications.Shared
{
    public class GetByIdSpec<T> : Specification<T> where T : BaseEntity<int>
    {
        public GetByIdSpec(int userId)
        {
            Query.Where(u => u.Id == userId);

        }
    }
}