using Gymify.Application.Common.CQRS;
using Gymify.Application.Security;
using Gymify.Application.Security.Markers;

namespace Gymify.Application.Features.Trainers.Queries.GetTrainerMe
{
    [Authorize]
    public class GetTrainerMeQuery : IQuery<GetTrainerMeQueryResponse>, IAuthorizedRequest
    {
    }
}
