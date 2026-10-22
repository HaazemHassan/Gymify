using AutoMapper;
using Gymify.Application.Features.Trainers.Queries.GetTrainersForManagement;

namespace Gymify.API.Requests.Trainers.GetTrainersForManagement
{
    public class GetTrainersForManagementRequestMappingProfile : Profile
    {
        public GetTrainersForManagementRequestMappingProfile()
        {
            CreateMap<GetTrainersForManagementRequest, GetTrainersForManagementQuery>()
                .ForMember(dest => dest.PageNumber, opt => opt.MapFrom(src => src.PageNumber ?? 1))
                .ForMember(dest => dest.PageSize, opt => opt.MapFrom(src => src.PageSize ?? 10))
                .ForMember(dest => dest.Search, opt => opt.MapFrom(src => src.Search))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive));
        }
    }
}
