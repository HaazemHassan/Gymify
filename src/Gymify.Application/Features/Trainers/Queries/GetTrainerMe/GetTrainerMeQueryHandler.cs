using ErrorOr;
using Gymify.Application.Common.CQRS;
using Gymify.Application.Contracts.Api;
using Gymify.Application.Features.Trainers.Queries.GetTrainerMe.Specifications;
using Gymify.Domain.Common.Constants;
using Gymify.Domain.RepositoriesContracts;

namespace Gymify.Application.Features.Trainers.Queries.GetTrainerMe
{
    public class GetTrainerMeQueryHandler : IQueryHandler<GetTrainerMeQuery, GetTrainerMeQueryResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public GetTrainerMeQueryHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task<ErrorOr<GetTrainerMeQueryResponse>> Handle(GetTrainerMeQuery request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId;
            if (!userId.HasValue)
            {
                return Error.Unauthorized(
                    code: ErrorCodes.Authorization.NotAuthenticated,
                    description: "User is not authenticated.");
            }

            var spec = new TrainerByDomainUserIdSpec(userId.Value);
            var trainer = await _unitOfWork.Trainers.GetAsync<GetTrainerMeQueryResponse>(spec, cancellationToken);

            if (trainer is null)
            {
                return Error.NotFound(
                    code: ErrorCodes.Trainer.TrainerNotFound,
                    description: "Trainer profile not found.");
            }

            return trainer;
        }
    }
}
