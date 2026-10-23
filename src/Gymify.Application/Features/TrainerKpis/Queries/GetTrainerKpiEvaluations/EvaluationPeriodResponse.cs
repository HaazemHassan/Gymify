namespace Gymify.Application.Features.TrainerKpis.Queries.GetTrainerKpiEvaluations
{
    public class EvaluationPeriodResponse
    {
        public DateOnly? From { get; set; }
        public DateOnly? To { get; set; }

        public EvaluationPeriodResponse()
        {
        }

        public EvaluationPeriodResponse(DateOnly? from, DateOnly? to)
        {
            From = from;
            To = to;
        }
    }
}
