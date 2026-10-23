using AutoMapper;
using Gymify.Domain.RepositoriesContracts;

namespace Gymify.Application.Features.TrainerKpis.Queries.GetTrainerDailyKpiEvaluations
{
    public class TrainerDailyEvaluationMappingProfile : Profile
    {
        public TrainerDailyEvaluationMappingProfile()
        {
            CreateMap<TrainerDailyEvaluationDto, TrainerDailyEvaluationResponse>();
            CreateMap<TrainerKpiDailyScoreDto, TrainerKpiDailyScoreItemResponse>();
        }
    }
}
