using AutoMapper;
using Gymify.Application.Features.Customers.Queries.GetCustomers;

namespace Gymify.API.Requests.Customers.GetCustomers
{
    public class GetCustomersRequestMappingProfile : Profile
    {
        public GetCustomersRequestMappingProfile()
        {
            CreateMap<GetCustomersRequest, GetCustomersQuery>()
                .ForMember(dest => dest.PageNumber, opt => opt.MapFrom(src => src.PageNumber ?? 1))
                .ForMember(dest => dest.PageSize, opt => opt.MapFrom(src => src.PageSize ?? 10))
                .ForMember(dest => dest.Search, opt => opt.MapFrom(src => src.Search));
        }
    }
}
