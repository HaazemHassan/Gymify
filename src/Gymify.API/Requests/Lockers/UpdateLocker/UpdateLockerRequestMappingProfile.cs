using AutoMapper;
using Gymify.Application.Features.Lockers.Commands.UpdateLocker;

namespace Gymify.API.Requests.Lockers.UpdateLocker
{
    public class UpdateLockerRequestMappingProfile : Profile
    {
        public UpdateLockerRequestMappingProfile()
        {
            CreateMap<UpdateLockerRequest, UpdateLockerCommand>();
        }
    }
}
