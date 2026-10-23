using AutoMapper;
using Gymify.Domain.Entities;
using Gymify.Domain.RepositoriesContracts;
using Microsoft.EntityFrameworkCore;

namespace Gymify.Infrastructure.Data.Repositories
{
    internal sealed class TrainerKpiScoreRepository(AppDbContext context, IMapper mapper)
        : GenericRepository<TrainerKpiScore>(context, mapper), ITrainerKpiScoreRepository
    {
        private readonly AppDbContext _context = context;

        public async Task<(decimal OverallAverage, int TotalCount, List<TrainerKpiAggregateResult> Kpis)> GetTrainerEvaluationsSummaryAsync(
            int trainerId,
            DateOnly? from,
            DateOnly? to,
            string? search,
            int pageNumber,
            int pageSize,
            CancellationToken ct = default)
        {
            var query = _context.TrainerKpiScores.AsNoTracking()
                .Where(s => s.TrainerId == trainerId);

            if (from.HasValue)
            {
                query = query.Where(s => s.Date >= from.Value);
            }

            if (to.HasValue)
            {
                query = query.Where(s => s.Date <= to.Value);
            }

            if (!string.IsNullOrWhiteSpace(search))
            {
                var term = search.Trim();
                query = query.Where(s => s.Kpi.Name.Contains(term));
            }

            var hasAny = await query.AnyAsync(ct);
            decimal overallAverage = 0m;
            if (hasAny)
            {
                overallAverage = Math.Round(await query.AverageAsync(s => s.Score, ct), 2);
            }

            var distinctKpiCount = await query.Select(s => s.KpiId).Distinct().CountAsync(ct);

            var pagedGroups = await query
                .GroupBy(s => new { s.KpiId, s.Kpi.Name })
                .Select(g => new
                {
                    g.Key.KpiId,
                    g.Key.Name,
                    AverageScore = g.Average(x => x.Score),
                    EvaluationCount = g.Count()
                })
                .OrderBy(x => x.Name)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(ct);

            var kpis = pagedGroups
                .Select(x => new TrainerKpiAggregateResult(
                    x.KpiId,
                    x.Name,
                    Math.Round(x.AverageScore, 2),
                    x.EvaluationCount
                ))
                .ToList();

            return (overallAverage, distinctKpiCount, kpis);
        }

        public async Task<(int TotalCount, List<TrainerDailyEvaluationDto> Data)> GetTrainerDailyEvaluationsAsync(
            int trainerId,
            DateOnly? date,
            DateOnly? from,
            DateOnly? to,
            int pageNumber,
            int pageSize,
            CancellationToken ct = default)
        {
            var query = _context.TrainerKpiScores.AsNoTracking()
                .Where(s => s.TrainerId == trainerId);

            if (date.HasValue)
            {
                query = query.Where(s => s.Date == date.Value);
            }
            else
            {
                if (from.HasValue)
                {
                    query = query.Where(s => s.Date >= from.Value);
                }

                if (to.HasValue)
                {
                    query = query.Where(s => s.Date <= to.Value);
                }
            }

            var datesQuery = query
                .Select(s => s.Date)
                .Distinct();

            var totalCount = await datesQuery.CountAsync(ct);

            if (totalCount == 0)
            {
                return (0, []);
            }

            var pagedDates = await datesQuery
                .OrderByDescending(d => d)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(ct);

            var scores = await query
                .Where(s => pagedDates.Contains(s.Date))
                .Select(s => new
                {
                    s.Date,
                    s.KpiId,
                    KpiName = s.Kpi.Name,
                    s.Score,
                    s.Comment
                })
                .OrderByDescending(s => s.Date)
                .ThenBy(s => s.KpiName)
                .ToListAsync(ct);

            var grouped = scores
                .GroupBy(s => s.Date)
                .OrderByDescending(g => g.Key)
                .Select(g => new TrainerDailyEvaluationDto
                {
                    Date = g.Key,
                    DailyAverage = Math.Round(g.Average(x => x.Score), 2),
                    Scores = g.Select(x => new TrainerKpiDailyScoreDto
                    {
                        KpiId = x.KpiId,
                        KpiName = x.KpiName,
                        Score = x.Score,
                        Comment = x.Comment
                    }).ToList()
                })
                .ToList();

            return (totalCount, grouped);
        }

        public async Task<List<TrainerKpiScore>> GetScoresByDayAsync(
            int trainerId,
            DateOnly day,
            CancellationToken ct = default)
        {
            return await _context.TrainerKpiScores
                .Where(s => s.TrainerId == trainerId && s.Date == day)
                .ToListAsync(ct);
        }
    }
}
