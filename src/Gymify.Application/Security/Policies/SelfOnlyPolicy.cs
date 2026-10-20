using Gymify.Application.Contracts.Api;
using Gymify.Application.Security.Markers;
using Gymify.Domain.Common.Constants;

namespace Gymify.Application.Security.Policies
{
    internal class SelfOnlyPolicy(ICurrentUserService _currentUserService) : IAuthorizationPolicy
    {
        public string Name => AuthorizationPolicies.SelfOnly;

        public ErrorOr<Success> Authorize(object request)
        {
            if (request is not IOwnedResourceRequest ownedRequest)
            {
                return Error.Validation(
                    code: ErrorCodes.Authorization.NotAllowed,
                    description: "Forbidden");
            }


            if (!_currentUserService.IsAuthenticated)

            {
                return Error.Unauthorized(code: ErrorCodes.Authorization.NotAuthenticated, description: "User not authenticated");
            }



            if (ownedRequest.UserId == _currentUserService.UserId)
            {
                return Result.Success;
            }

            return Error.Forbidden(code: ErrorCodes.Authorization.NotAllowed, description: "You are not allowed to perform this action on this resource.");
        }
    }
}
