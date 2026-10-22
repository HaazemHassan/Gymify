using AutoMapper;
using Gymify.Application.Features.Packages.Queries.GetPackagesForCustomers;

namespace Gymify.API.Requests.Packages.GetPackagesForCustomers
{
    public class GetPackagesForCustomersRequestMappingProfile : Profile
    {
        public GetPackagesForCustomersRequestMappingProfile()
        {
            CreateMap<GetPackagesForCustomersRequest, GetPackagesForCustomersQuery>()
                .ForMember(dest => dest.PageNumber, opt => opt.MapFrom(src => src.PageNumber ?? 1))
                .ForMember(dest => dest.PageSize, opt => opt.MapFrom(src => src.PageSize ?? 10))
                .ForMember(dest => dest.Search, opt => opt.MapFrom(src => src.Search))
                .ForMember(dest => dest.Types, opt => opt.MapFrom(src => src.Types));
        }
    }
}
