using Gymify.Application.Security;
using Gymify.Application.Security.Markers;

namespace Gymify.Application.Features.Users.Queries.GetMe
{
    [Authorize]
    public class GetMeQuery : IQuery<GetMeQueryResponse>, IAuthorizedRequest
    {
    }
}
