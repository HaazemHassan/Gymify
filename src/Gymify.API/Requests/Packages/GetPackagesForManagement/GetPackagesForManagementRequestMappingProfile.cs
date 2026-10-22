using AutoMapper;
using Gymify.Application.Features.Packages.Queries.GetPackagesForManagement;

namespace Gymify.API.Requests.Packages.GetPackagesForManagement
{
    public class GetPackagesForManagementRequestMappingProfile : Profile
    {
        public GetPackagesForManagementRequestMappingProfile()
        {
            CreateMap<GetPackagesForManagementRequest, GetPackagesForManagementQuery>()
                .ForMember(dest => dest.PageNumber, opt => opt.MapFrom(src => src.PageNumber ?? 1))
                .ForMember(dest => dest.PageSize, opt => opt.MapFrom(src => src.PageSize ?? 10))
                .ForMember(dest => dest.Search, opt => opt.MapFrom(src => src.Search))
                .ForMember(dest => dest.Types, opt => opt.MapFrom(src => src.Types))
                .ForMember(dest => dest.TrainerId, opt => opt.MapFrom(src => src.TrainerId))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive));
        }
    }
}
