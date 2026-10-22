using ErrorOr;
using Gymify.Application.Contracts.Api;
using Gymify.Application.Security.Markers;
using Gymify.Domain.Common.Constants;
using Gymify.Domain.RepositoriesContracts;

namespace Gymify.Application.Security.Policies
{
    internal class SelfCustomerOnlyPolicy(
        ICurrentUserService _currentUserService,
        IUnitOfWork _unitOfWork) : IAuthorizationPolicy
    {
        public string Name => AuthorizationPolicies.SelfCustomerOnly;

        public ErrorOr<Success> Authorize(object request)
        {
            if (request is not ICustomerResourceRequest customerResourceRequest)
            {
                return Error.Validation(
                    code: ErrorCodes.Authorization.NotAllowed,
                    description: "Forbidden");
            }

            if (!_currentUserService.IsAuthenticated || !_currentUserService.UserId.HasValue)
            {
                return Error.Unauthorized(
                    code: ErrorCodes.Authorization.NotAuthenticated,
                    description: "User not authenticated");
            }

            var currentUserId = _currentUserService.UserId.Value;

            var isOwner = _unitOfWork.Customers
                .AsQueryable()
                .Any(c => c.Id == customerResourceRequest.CustomerId && c.DomainUserId == currentUserId);

            if (isOwner)
            {
                return Result.Success;
            }

            return Error.Forbidden(
                code: ErrorCodes.Authorization.NotAllowed,
                description: "You are not allowed to perform this action on this resource.");
        }
    }
}
