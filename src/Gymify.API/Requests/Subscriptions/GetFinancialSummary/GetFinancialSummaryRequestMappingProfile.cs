using AutoMapper;
using Gymify.Application.Features.Subscriptions.Queries.GetFinancialSummary;

namespace Gymify.API.Requests.Subscriptions.GetFinancialSummary
{
    public class GetFinancialSummaryRequestMappingProfile : Profile
    {
        public GetFinancialSummaryRequestMappingProfile()
        {
            CreateMap<GetFinancialSummaryRequest, GetFinancialSummaryQuery>();
        }
    }
}
