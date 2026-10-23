using AutoMapper;
using Gymify.Application.Features.Trainers.Commands.CreateTrainer;

namespace Gymify.API.Requests.Trainers.CreateTrainer
{
    public class CreateTrainerRequestMappingProfile : Profile
    {
        public CreateTrainerRequestMappingProfile()
        {
            CreateMap<CreateTrainerRequest, CreateTrainerCommand>();
        }
    }
}
