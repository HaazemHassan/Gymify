using ErrorOr;
using Gymify.Application.Contracts.Api;
using Gymify.Application.Security.Markers;
using Gymify.Domain.Common.Constants;
using Gymify.Domain.RepositoriesContracts;

namespace Gymify.Application.Security.Policies
{
    internal class SelfTrainerOnlyPolicy(
        ICurrentUserService _currentUserService,
        IUnitOfWork _unitOfWork) : IAuthorizationPolicy
    {
        public string Name => AuthorizationPolicies.SelfTrainerOnly;

        public ErrorOr<Success> Authorize(object request)
        {
            if (request is not ITrainerResourceRequest trainerResourceRequest)
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

            var isOwner = _unitOfWork.Trainers
                .AsQueryable()
                .Any(t => t.Id == trainerResourceRequest.TrainerId 
                       && t.Employee.DomainUserId == currentUserId
                       && t.Employee.DomainUser.IsActive);

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
