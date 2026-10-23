using AutoMapper;
using ErrorOr;
using Gymify.Application.Common.CQRS;
using Gymify.Application.Common.Pagination;
using Gymify.Domain.Common.Constants;
using Gymify.Domain.RepositoriesContracts;

namespace Gymify.Application.Features.TrainerKpis.Queries.GetTrainerDailyKpiEvaluations
{
    public class GetTrainerDailyKpiEvaluationsQueryHandler
        : IQueryHandler<GetTrainerDailyKpiEvaluationsQuery, PaginatedResult<TrainerDailyEvaluationResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetTrainerDailyKpiEvaluationsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ErrorOr<PaginatedResult<TrainerDailyEvaluationResponse>>> Handle(
            GetTrainerDailyKpiEvaluationsQuery request,
            CancellationToken cancellationToken)
        {
            var trainerExists = await _unitOfWork.Trainers.AnyAsync(t => t.Id == request.TrainerId, cancellationToken);
            if (!trainerExists)
            {
                return Error.NotFound(ErrorCodes.Trainer.TrainerNotFound, "Trainer was not found.");
            }

            var (totalCount, dtos) = await _unitOfWork.TrainerKpiScores.GetTrainerDailyEvaluationsAsync(
                request.TrainerId,
                request.Date,
                request.From,
                request.To,
                request.PageNumber,
                request.PageSize,
                cancellationToken);

            var responseList = _mapper.Map<List<TrainerDailyEvaluationResponse>>(dtos);

            return new PaginatedResult<TrainerDailyEvaluationResponse>(
                responseList,
                totalCount,
                request.PageNumber,
                request.PageSize);
        }
    }
}
