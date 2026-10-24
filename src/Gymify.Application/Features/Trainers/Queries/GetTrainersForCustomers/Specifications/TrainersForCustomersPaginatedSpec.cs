using Ardalis.Specification;
using Gymify.Application.Features.Trainers.Specifications;
using Gymify.Application.Specifications;

namespace Gymify.Application.Features.Trainers.Queries.GetTrainersForCustomers.Specifications
{
    public class TrainersForCustomersPaginatedSpec : TrainersFilteredSpec
    {
        public TrainersForCustomersPaginatedSpec(int pageNumber, int pageSize, string? search = null)
            : base(search, isAvailable: true, isActive: true)
        {
            Query.OrderByDescending(t => t.YearsOfExperience)
                .ThenBy(t => t.Employee.DomainUser.Name);

            Query.Paginate(pageNumber, pageSize);
            Query.AsNoTracking();
        }
    }
}
