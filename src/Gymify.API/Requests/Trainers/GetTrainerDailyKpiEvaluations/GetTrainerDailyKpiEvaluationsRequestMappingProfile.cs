using AutoMapper;
using Gymify.Application.Features.TrainerKpis.Queries.GetTrainerDailyKpiEvaluations;

namespace Gymify.API.Requests.Trainers.GetTrainerDailyKpiEvaluations
{
    public class GetTrainerDailyKpiEvaluationsRequestMappingProfile : Profile
    {
        public GetTrainerDailyKpiEvaluationsRequestMappingProfile()
        {
            CreateMap<GetTrainerDailyKpiEvaluationsRequest, GetTrainerDailyKpiEvaluationsQuery>()
                .ForMember(dest => dest.PageNumber, opt => opt.MapFrom(src => src.PageNumber ?? 1))
                .ForMember(dest => dest.PageSize, opt => opt.MapFrom(src => src.PageSize ?? 10))
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date))
                .ForMember(dest => dest.From, opt => opt.MapFrom(src => src.From))
                .ForMember(dest => dest.To, opt => opt.MapFrom(src => src.To));
        }
    }
}
