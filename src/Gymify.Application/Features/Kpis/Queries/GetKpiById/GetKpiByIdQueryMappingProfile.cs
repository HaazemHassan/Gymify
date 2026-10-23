using AutoMapper;
using Gymify.Domain.Entities;

namespace Gymify.Application.Features.Kpis.Queries.GetKpiById
{
    public class GetKpiByIdQueryMappingProfile : Profile
    {
        public GetKpiByIdQueryMappingProfile()
        {
            CreateMap<Kpi, GetKpiByIdQueryResponse>();
        }
    }
}
