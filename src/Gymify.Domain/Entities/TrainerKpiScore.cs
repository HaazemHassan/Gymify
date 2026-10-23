using Gymify.Domain.Entities.Base;

namespace Gymify.Domain.Entities
{
    public sealed class TrainerKpiScore : FullAuditableEntity<int>
    {
        private TrainerKpiScore()
        {
        }

        public TrainerKpiScore(
            int trainerId,
            int kpiId,
            decimal score,
            DateOnly date,
            string? comment = null)
        {
            TrainerId = trainerId;
            KpiId = kpiId;
            Score = score;
            Date = date;
            Comment = comment;
        }

        public int TrainerId { get; private set; }
        public Trainer Trainer { get; private set; } = null!;

        public int KpiId { get; private set; }
        public Kpi Kpi { get; private set; } = null!;

        public decimal Score { get; private set; }
        public string? Comment { get; private set; }
        public DateOnly Date { get; private set; }

        public void Update(decimal? score = null, string? comment = null)
        {
            if (score.HasValue)
            {
                Score = score.Value;
            }

            Comment = comment;
        }
    }
}
