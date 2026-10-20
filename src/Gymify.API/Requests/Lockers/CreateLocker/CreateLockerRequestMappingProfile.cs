using AutoMapper;
using Gymify.Application.Features.Lockers.Commands.CreateLocker;

namespace Gymify.API.Requests.Lockers.CreateLocker
{
    public class CreateLockerRequestMappingProfile : Profile
    {
        public CreateLockerRequestMappingProfile()
        {
            CreateMap<CreateLockerRequest, CreateLockerCommand>();
        }
    }
}
