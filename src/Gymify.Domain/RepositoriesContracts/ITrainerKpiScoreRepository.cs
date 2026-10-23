using Gymify.Domain.Entities;
using Gymify.Domain.Repositories;

namespace Gymify.Domain.RepositoriesContracts
{
    public class TrainerKpiAggregateResult
    {
        public int KpiId { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal AverageScore { get; set; }
        public int EvaluationCount { get; set; }

        public TrainerKpiAggregateResult()
        {
        }

        public TrainerKpiAggregateResult(int kpiId, string name, decimal averageScore, int evaluationCount)
        {
            KpiId = kpiId;
            Name = name;
            AverageScore = averageScore;
            EvaluationCount = evaluationCount;
        }
    }

    public class TrainerKpiDailyScoreDto
    {
        public int KpiId { get; set; }
        public string KpiName { get; set; } = string.Empty;
        public decimal Score { get; set; }
        public string? Comment { get; set; }
    }

    public class TrainerDailyEvaluationDto
    {
        public DateOnly Date { get; set; }
        public decimal DailyAverage { get; set; }
        public List<TrainerKpiDailyScoreDto> Scores { get; set; } = [];
    }

    public interface ITrainerKpiScoreRepository : IGenericRepository<TrainerKpiScore>
    {
        Task<(decimal OverallAverage, int TotalCount, List<TrainerKpiAggregateResult> Kpis)> GetTrainerEvaluationsSummaryAsync(
            int trainerId,
            DateOnly? from,
            DateOnly? to,
            string? search,
            int pageNumber,
            int pageSize,
            CancellationToken ct = default);

        Task<(int TotalCount, List<TrainerDailyEvaluationDto> Data)> GetTrainerDailyEvaluationsAsync(
            int trainerId,
            DateOnly? date,
            DateOnly? from,
            DateOnly? to,
            int pageNumber,
            int pageSize,
            CancellationToken ct = default);

        Task<List<TrainerKpiScore>> GetScoresByDayAsync(
            int trainerId,
            DateOnly day,
            CancellationToken ct = default);
    }
}
