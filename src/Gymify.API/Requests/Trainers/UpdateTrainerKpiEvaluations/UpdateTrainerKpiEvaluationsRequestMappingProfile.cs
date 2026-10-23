using AutoMapper;
using Gymify.Application.Features.TrainerKpis.Commands.UpdateTrainerKpiEvaluations;

namespace Gymify.API.Requests.Trainers.UpdateTrainerKpiEvaluations
{
    public class UpdateTrainerKpiEvaluationsRequestMappingProfile : Profile
    {
        public UpdateTrainerKpiEvaluationsRequestMappingProfile()
        {
            CreateMap<UpdateTrainerKpiEvaluationsRequest, UpdateTrainerKpiEvaluationsCommand>();
        }
    }
}
