using PrimeFit.Application.Common.Enums;

namespace PrimeFit.Application.Features.Branches.Queries.GetOwnerBranchesStatistics
{
    public class GetOwnerBranchesStatisticsQuery : IQuery<GetOwnerBranchesStatisticsQueryResponse>
    {
        public TimePeriod? TimePeriod { get; set; }
    }
}
