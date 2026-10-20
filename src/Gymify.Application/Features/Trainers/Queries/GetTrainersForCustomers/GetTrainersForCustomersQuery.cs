using Gymify.Application.Common.CQRS;
using Gymify.Application.Common.Pagination;

namespace Gymify.Application.Features.Trainers.Queries.GetTrainersForCustomers
{
    public class GetTrainersForCustomersQuery : PaginatedQuery, IQuery<PaginatedResult<GetTrainersForCustomersQueryResponse>>
    {
    }
}
