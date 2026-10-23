using AutoMapper;
using Gymify.Application.Features.Trainers.Queries.GetTrainerCustomers;

namespace Gymify.API.Requests.Trainers.GetTrainerCustomers
{
    public class GetTrainerCustomersRequestMappingProfile : Profile
    {
        public GetTrainerCustomersRequestMappingProfile()
        {
            CreateMap<GetTrainerCustomersRequest, GetTrainerCustomersQuery>()
                .ForMember(dest => dest.PageNumber, opt => opt.MapFrom(src => src.PageNumber ?? 1))
                .ForMember(dest => dest.PageSize, opt => opt.MapFrom(src => src.PageSize ?? 10))
                .ForMember(dest => dest.Search, opt => opt.MapFrom(src => src.Search));
        }
    }
}
