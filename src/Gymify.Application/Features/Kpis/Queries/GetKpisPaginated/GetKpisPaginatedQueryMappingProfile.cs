using AutoMapper;
using Gymify.Domain.Entities;

namespace Gymify.Application.Features.Kpis.Queries.GetKpisPaginated
{
    public class GetKpisPaginatedQueryMappingProfile : Profile
    {
        public GetKpisPaginatedQueryMappingProfile()
        {
            CreateMap<Kpi, GetKpisPaginatedQueryResponse>();
        }
    }
}
