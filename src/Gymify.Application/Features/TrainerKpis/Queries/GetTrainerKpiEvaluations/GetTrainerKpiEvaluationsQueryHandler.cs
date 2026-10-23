using ErrorOr;
using Gymify.Application.Common.CQRS;
using Gymify.Domain.Common.Constants;
using Gymify.Domain.RepositoriesContracts;

namespace Gymify.Application.Features.TrainerKpis.Queries.GetTrainerKpiEvaluations
{
    public class GetTrainerKpiEvaluationsQueryHandler : IQueryHandler<GetTrainerKpiEvaluationsQuery, GetTrainerKpiEvaluationsQueryResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetTrainerKpiEvaluationsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ErrorOr<GetTrainerKpiEvaluationsQueryResponse>> Handle(GetTrainerKpiEvaluationsQuery request, CancellationToken cancellationToken)
        {
            var trainerExists = await _unitOfWork.Trainers.AnyAsync(t => t.Id == request.TrainerId, cancellationToken);
            if (!trainerExists)
            {
                return Error.NotFound(
                    code: ErrorCodes.Trainer.TrainerNotFound,
                    description: "Trainer was not found.");
            }

            var (overallAverage, totalCount, kpiAggregates) = await _unitOfWork.TrainerKpiScores.GetTrainerEvaluationsSummaryAsync(
                request.TrainerId,
                request.From,
                request.To,
                request.Search,
                request.PageNumber,
                request.PageSize,
                cancellationToken);

            var kpisList = kpiAggregates.Select(k => new TrainerKpiAverageItemResponse
            {
                KpiId = k.KpiId,
                Name = k.Name,
                AverageScore = k.AverageScore,
                EvaluationCount = k.EvaluationCount
            }).ToList();

            var totalPages = (int)Math.Ceiling(totalCount / (double)request.PageSize);

            return new GetTrainerKpiEvaluationsQueryResponse
            {
                TrainerId = request.TrainerId,
                Period = new EvaluationPeriodResponse(request.From, request.To),
                OverallAverage = overallAverage,
                Kpis = kpisList,
                CurrentPage = request.PageNumber,
                PageSize = request.PageSize,
                TotalPages = totalPages,
                TotalCount = totalCount
            };
        }
    }
}
