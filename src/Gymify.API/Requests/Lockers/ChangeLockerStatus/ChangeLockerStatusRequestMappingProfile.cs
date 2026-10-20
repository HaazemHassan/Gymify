using AutoMapper;
using Gymify.Application.Features.Lockers.Commands.ChangeLockerStatus;

namespace Gymify.API.Requests.Lockers.ChangeLockerStatus
{
    public class ChangeLockerStatusRequestMappingProfile : Profile
    {
        public ChangeLockerStatusRequestMappingProfile()
        {
            CreateMap<ChangeLockerStatusRequest, ChangeLockerStatusCommand>();
        }
    }
}
