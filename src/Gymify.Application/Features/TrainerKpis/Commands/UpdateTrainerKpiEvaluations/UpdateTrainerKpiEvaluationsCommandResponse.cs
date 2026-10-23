namespace Gymify.Application.Features.TrainerKpis.Commands.UpdateTrainerKpiEvaluations
{
    public class UpdateTrainerKpiEvaluationsCommandResponse
    {
        public int TrainerId { get; set; }
        public DateOnly Date { get; set; }
        public int UpdatedCount { get; set; }

        public UpdateTrainerKpiEvaluationsCommandResponse() { }

        public UpdateTrainerKpiEvaluationsCommandResponse(int trainerId, DateOnly date, int updatedCount)
        {
            TrainerId = trainerId;
            Date = date;
            UpdatedCount = updatedCount;
        }
    }
}
