using Gymify.Application.Common.CQRS;
using Gymify.Application.Common.Pagination;
using Gymify.Application.Security;
using Gymify.Application.Security.Markers;
using Gymify.Application.Security.Policies;
using Gymify.Domain.Common.Enums;

namespace Gymify.Application.Features.TrainerKpis.Queries.GetTrainerKpiEvaluations
{
    [Authorize(Permissions = [Permission.TrainersRead])]
    [Authorize(Policy = AuthorizationPolicies.SelfTrainerOnly)]
    public class GetTrainerKpiEvaluationsQuery : PaginatedQuery, IQuery<GetTrainerKpiEvaluationsQueryResponse>, ITrainerResourceRequest
    {
        public int TrainerId { get; set; }
        public DateOnly? From { get; set; }
        public DateOnly? To { get; set; }
    }
}
