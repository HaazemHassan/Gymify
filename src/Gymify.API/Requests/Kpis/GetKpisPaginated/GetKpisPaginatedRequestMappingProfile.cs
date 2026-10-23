using AutoMapper;
using Gymify.Application.Features.Kpis.Queries.GetKpisPaginated;

namespace Gymify.API.Requests.Kpis.GetKpisPaginated
{
    public class GetKpisPaginatedRequestMappingProfile : Profile
    {
        public GetKpisPaginatedRequestMappingProfile()
        {
            CreateMap<GetKpisPaginatedRequest, GetKpisPaginatedQuery>()
                .ForMember(dest => dest.PageNumber, opt => opt.MapFrom(src => src.PageNumber ?? 1))
                .ForMember(dest => dest.PageSize, opt => opt.MapFrom(src => src.PageSize ?? 10))
                .ForMember(dest => dest.Search, opt => opt.MapFrom(src => src.Search))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive));
        }
    }
}
