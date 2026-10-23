using AutoMapper;
using Gymify.Application.Features.TrainerKpis.Queries.GetTrainerKpiEvaluations;

namespace Gymify.API.Requests.Trainers.GetTrainerKpiEvaluations
{
    public class GetTrainerKpiEvaluationsRequestMappingProfile : Profile
    {
        public GetTrainerKpiEvaluationsRequestMappingProfile()
        {
            CreateMap<GetTrainerKpiEvaluationsRequest, GetTrainerKpiEvaluationsQuery>()
                .ForMember(dest => dest.PageNumber, opt => opt.MapFrom(src => src.PageNumber ?? 1))
                .ForMember(dest => dest.PageSize, opt => opt.MapFrom(src => src.PageSize ?? 10))
                .ForMember(dest => dest.From, opt => opt.MapFrom(src => src.From))
                .ForMember(dest => dest.To, opt => opt.MapFrom(src => src.To));
        }
    }
}
