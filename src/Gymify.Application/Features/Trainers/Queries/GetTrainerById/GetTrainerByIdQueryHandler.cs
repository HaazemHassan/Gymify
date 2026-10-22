using ErrorOr;
using Gymify.Application.Common.CQRS;
using Gymify.Application.Features.Trainers.Queries.GetTrainerById.Specifications;
using Gymify.Domain.Common.Constants;
using Gymify.Domain.RepositoriesContracts;

namespace Gymify.Application.Features.Trainers.Queries.GetTrainerById
{
    public class GetTrainerByIdQueryHandler : IQueryHandler<GetTrainerByIdQuery, GetTrainerByIdQueryResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetTrainerByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ErrorOr<GetTrainerByIdQueryResponse>> Handle(GetTrainerByIdQuery request, CancellationToken cancellationToken)
        {
            var spec = new TrainerDetailsByIdSpec(request.Id);
            var trainer = await _unitOfWork.Trainers.GetAsync<GetTrainerByIdQueryResponse>(spec, cancellationToken);

            if (trainer is null)
            {
                return Error.NotFound(
                    code: ErrorCodes.Trainer.TrainerNotFound,
                    description: $"Trainer was not found.");
            }

            return trainer;
        }
    }
}
