using Gymify.Application.Contracts.Api;
using Gymify.Domain.Common.Constants;
using Gymify.Domain.Common.Enums;
using Gymify.Domain.RepositoriesContracts;

namespace Gymify.Application.Features.Users.Queries.GetMe
{
    public class GetMeQueryHandler : IQueryHandler<GetMeQuery, GetMeQueryResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public GetMeQueryHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task<ErrorOr<GetMeQueryResponse>> Handle(GetMeQuery request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId;
            if (!userId.HasValue)
            {
                return Error.Unauthorized(code: ErrorCodes.Authentication.InvalidAccessToken, description: "Invalid access token");
            }

            var user = await _unitOfWork.Users.GetAsync(u => u.Id == userId.Value, cancellationToken);
            if (user is null)
            {
                return Error.NotFound(code: ErrorCodes.User.UserNotFound, description: "User not found");
            }

            var customer = await _unitOfWork.Customers.GetAsync(c => c.DomainUserId == userId.Value, cancellationToken);

            var roles = _currentUserService.GetRoles();
            UserRole? userRole = roles.Count > 0 ? roles[0] : null;

            return new GetMeQueryResponse
            {
                Id = userId.Value,
                Name = user.Name,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                UserType = user.UserType,
                UserRole = userRole,
                ApprovalStatus = customer?.ApprovalStatus,
                IsActive = user.IsActive
            };
        }
    }
}
