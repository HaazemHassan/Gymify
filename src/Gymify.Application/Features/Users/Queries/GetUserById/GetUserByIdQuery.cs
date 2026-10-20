using Gymify.Application.Security;
using Gymify.Application.Security.Markers;
using Gymify.Application.Security.Policies;

namespace Gymify.Application.Features.Users.Queries.GetUserById
{
    [Authorize(Policy = AuthorizationPolicies.SelfOnly)]
    public class GetUserByIdQuery : IQuery<GetUserByIdQueryResponse>, IOwnedResourceRequest
    {
        public GetUserByIdQuery(int userId)
        {
            UserId = userId;
        }

        public int UserId { get; set; }
    }
}
