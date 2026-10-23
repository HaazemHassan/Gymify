using AutoMapper;
using Gymify.Application.Features.TrainerKpis.Commands.SubmitTrainerKpiEvaluations;

namespace Gymify.API.Requests.Trainers.SubmitTrainerKpiEvaluations
{
    public class SubmitTrainerKpiEvaluationsRequestMappingProfile : Profile
    {
        public SubmitTrainerKpiEvaluationsRequestMappingProfile()
        {
            CreateMap<SubmitTrainerKpiEvaluationsRequest, SubmitTrainerKpiEvaluationsCommand>();
        }
    }
}
