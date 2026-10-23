using AutoMapper;
using Gymify.Application.Features.CustomerBehaviorNotes.Queries.GetCustomerBehaviorNotes;

namespace Gymify.API.Requests.Customers.GetCustomerBehaviorNotes
{
    public class GetCustomerBehaviorNotesRequestMappingProfile : Profile
    {
        public GetCustomerBehaviorNotesRequestMappingProfile()
        {
            CreateMap<GetCustomerBehaviorNotesRequest, GetCustomerBehaviorNotesQuery>()
                .ForMember(dest => dest.PageNumber, opt => opt.MapFrom(src => src.PageNumber ?? 1))
                .ForMember(dest => dest.PageSize, opt => opt.MapFrom(src => src.PageSize ?? 10))
                .ForMember(dest => dest.Search, opt => opt.MapFrom(src => src.Search));
        }
    }
}
