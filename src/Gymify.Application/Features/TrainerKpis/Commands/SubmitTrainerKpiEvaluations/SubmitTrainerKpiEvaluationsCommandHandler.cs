using ErrorOr;
using Gymify.Application.Common;
using Gymify.Application.Common.CQRS;
using Gymify.Application.ServicesContracts.Infrastructure;
using Gymify.Domain.Common.Constants;
using Gymify.Domain.Entities;
using Gymify.Domain.RepositoriesContracts;

namespace Gymify.Application.Features.TrainerKpis.Commands.SubmitTrainerKpiEvaluations
{
    public class SubmitTrainerKpiEvaluationsCommandHandler
        : ICommandHandler<SubmitTrainerKpiEvaluationsCommand, SubmitTrainerKpiEvaluationsCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDateTimeProvider _dateTimeProvider;

        public SubmitTrainerKpiEvaluationsCommandHandler(
            IUnitOfWork unitOfWork,
            IDateTimeProvider dateTimeProvider)
        {
            _unitOfWork = unitOfWork;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<ErrorOr<SubmitTrainerKpiEvaluationsCommandResponse>> Handle(
            SubmitTrainerKpiEvaluationsCommand request,
            CancellationToken cancellationToken)
        {
            var trainerExists = await _unitOfWork.Trainers.AnyAsync(t => t.Id == request.TrainerId, cancellationToken);
            if (!trainerExists)
            {
                return Error.NotFound(
                    code: ErrorCodes.Trainer.TrainerNotFound,
                    description: "Active trainer was not found.");
            }

            var today = DateOnly.FromDateTime(_dateTimeProvider.UtcNow.Date);

            var alreadyEvaluatedToday = await _unitOfWork.TrainerKpiScores.AnyAsync(
                s => s.TrainerId == request.TrainerId && s.Date == today,
                cancellationToken);

            if (alreadyEvaluatedToday)
            {
                return Error.Conflict(
                    code: ErrorCodes.TrainerKpi.AlreadyEvaluatedToday,
                    description: "This trainer has already been evaluated today.");
            }

            var kpiIds = request.Scores.Select(s => s.KpiId).ToList();
            var existingActiveKpiCount = await _unitOfWork.Kpis.CountAsync(
                k => kpiIds.Contains(k.Id) && k.IsActive,
                cancellationToken);

            if (existingActiveKpiCount != kpiIds.Count)
            {
                return Error.Validation(
                    code: ErrorCodes.TrainerKpi.InvalidScore,
                    description: "One or more specified KPIs do not exist or are inactive.");
            }

            foreach (var scoreItem in request.Scores)
            {
                var scoreEntity = new TrainerKpiScore(
                    trainerId: request.TrainerId,
                    kpiId: scoreItem.KpiId,
                    score: scoreItem.Score,
                    date: today,
                    comment: scoreItem.Comment);

                await _unitOfWork.TrainerKpiScores.AddAsync(scoreEntity, cancellationToken);
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new SubmitTrainerKpiEvaluationsCommandResponse(request.TrainerId, today, request.Scores.Count);
        }
    }
}
