using Ardalis.Specification;
using Gymify.Application.Features.Trainers.Specifications;
using Gymify.Application.Specifications;

namespace Gymify.Application.Features.Trainers.Queries.GetTrainersForManagement.Specifications
{
    public class TrainersForManagementPaginatedSpec : TrainersFilteredSpec
    {
        public TrainersForManagementPaginatedSpec(int pageNumber, int pageSize, string? search = null, bool? isActive = null)
            : base(search, isActive: isActive)
        {
            Query.OrderByDescending(t => t.CreatedAt);
            Query.Paginate(pageNumber, pageSize);
            Query.AsNoTracking();
        }
    }
}
