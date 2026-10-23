namespace Gymify.Application.Features.TrainerKpis.Commands.SubmitTrainerKpiEvaluations
{
    public class SubmitTrainerKpiEvaluationsCommandResponse
    {
        public int TrainerId { get; set; }
        public DateOnly Date { get; set; }
        public int ScoresCount { get; set; }

        public SubmitTrainerKpiEvaluationsCommandResponse() { }

        public SubmitTrainerKpiEvaluationsCommandResponse(int trainerId, DateOnly date, int scoresCount)
        {
            TrainerId = trainerId;
            Date = date;
            ScoresCount = scoresCount;
        }
    }
}
