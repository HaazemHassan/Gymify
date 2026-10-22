using AutoMapper;
using Gymify.Application.Features.Trainers.Commands.UpdateTrainer;

namespace Gymify.API.Requests.Trainers.UpdateTrainer
{
    public class UpdateTrainerRequestMappingProfile : Profile
    {
        public UpdateTrainerRequestMappingProfile()
        {
            CreateMap<UpdateTrainerRequest, UpdateTrainerCommand>();
        }
    }
}
