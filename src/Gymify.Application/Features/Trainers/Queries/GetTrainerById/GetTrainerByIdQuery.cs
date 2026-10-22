using Gymify.Application.Common.CQRS;
using Gymify.Application.Security;
using Gymify.Application.Security.Markers;
using Gymify.Application.Security.Policies;
using Gymify.Domain.Common.Enums;

namespace Gymify.Application.Features.Trainers.Queries.GetTrainerById
{
    [Authorize(Permissions = [Permission.TrainersRead])]
    [Authorize(Policy = AuthorizationPolicies.SelfTrainerOnly)]
    public class GetTrainerByIdQuery : IQuery<GetTrainerByIdQueryResponse>, ITrainerResourceRequest
    {
        public int Id { get; set; }

        public int TrainerId => Id;

        public GetTrainerByIdQuery(int id)
        {
            Id = id;
        }
    }
}
