using ErrorOr;
using Gymify.Application.Common.CQRS;
using Gymify.Domain.Common.Constants;
using Gymify.Domain.Entities;
using Gymify.Domain.RepositoriesContracts;

namespace Gymify.Application.Features.TrainerKpis.Commands.UpdateTrainerKpiEvaluations
{
    public class UpdateTrainerKpiEvaluationsCommandHandler
        : ICommandHandler<UpdateTrainerKpiEvaluationsCommand, UpdateTrainerKpiEvaluationsCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateTrainerKpiEvaluationsCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ErrorOr<UpdateTrainerKpiEvaluationsCommandResponse>> Handle(
            UpdateTrainerKpiEvaluationsCommand request,
            CancellationToken cancellationToken)
        {
            var trainerExists = await _unitOfWork.Trainers.AnyAsync(t => t.Id == request.TrainerId, cancellationToken);
            if (!trainerExists)
            {
                return Error.NotFound(ErrorCodes.Trainer.TrainerNotFound, "Trainer was not found.");
            }

            var existingScores = await _unitOfWork.TrainerKpiScores.GetScoresByDayAsync(
                request.TrainerId,
                request.Date,
                cancellationToken);

            if (existingScores.Count == 0)
            {
                return Error.NotFound(
                    ErrorCodes.TrainerKpi.ScoreNotFound,
                    $"No KPI evaluations found for trainer on date {request.Date:yyyy-MM-dd}.");
            }

            var requestedKpiIds = request.Scores.Select(s => s.KpiId).Distinct().ToList();
            var activeKpisCount = await _unitOfWork.Kpis.CountAsync(
                k => requestedKpiIds.Contains(k.Id) && k.IsActive,
                cancellationToken);

            if (activeKpisCount != requestedKpiIds.Count)
            {
                return Error.Validation(
                    code: ErrorCodes.TrainerKpi.InvalidScore,
                    description: "One or more specified KPIs do not exist or are inactive.");
            }

            // Delete any existing scores for this date that are not included in the updated scores list
            var scoresToRemove = existingScores.Where(e => !requestedKpiIds.Contains(e.KpiId)).ToList();
            if (scoresToRemove.Count > 0)
            {
                await _unitOfWork.TrainerKpiScores.DeleteRangeAsync(scoresToRemove, cancellationToken);
            }

            // Update existing or add newly specified KPIs
            var existingMap = existingScores.ToDictionary(e => e.KpiId);
            foreach (var scoreItem in request.Scores)
            {
                if (existingMap.TryGetValue(scoreItem.KpiId, out var existingScore))
                {
                    existingScore.Update(scoreItem.Score, scoreItem.Comment);
                    await _unitOfWork.TrainerKpiScores.UpdateAsync(existingScore, cancellationToken);
                }
                else
                {
                    var newScore = new TrainerKpiScore(
                        trainerId: request.TrainerId,
                        kpiId: scoreItem.KpiId,
                        score: scoreItem.Score,
                        date: request.Date,
                        comment: scoreItem.Comment);

                    await _unitOfWork.TrainerKpiScores.AddAsync(newScore, cancellationToken);
                }
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new UpdateTrainerKpiEvaluationsCommandResponse(request.TrainerId, request.Date, request.Scores.Count);
        }
    }
}
