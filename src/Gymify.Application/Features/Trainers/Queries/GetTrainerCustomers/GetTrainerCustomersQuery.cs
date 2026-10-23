using Gymify.Application.Common.CQRS;
using Gymify.Application.Common.Pagination;
using Gymify.Application.Security;
using Gymify.Application.Security.Markers;
using Gymify.Application.Security.Policies;
using Gymify.Domain.Common.Enums;

namespace Gymify.Application.Features.Trainers.Queries.GetTrainerCustomers
{
    [Authorize(Permissions = [Permission.TrainersRead])]
    [Authorize(Policy = AuthorizationPolicies.SelfTrainerOnly)]
    public class GetTrainerCustomersQuery : PaginatedQuery, IQuery<PaginatedResult<GetTrainerCustomersQueryResponse>>, ITrainerResourceRequest
    {
        public int TrainerId { get; set; }
    }
}
