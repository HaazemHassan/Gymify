using AutoMapper;
using Gymify.Application.Features.Attendances.Commands.CheckIn;

namespace Gymify.API.Requests.Attendances.CheckIn
{
    public class CheckInRequestMappingProfile : Profile
    {
        public CheckInRequestMappingProfile()
        {
            CreateMap<CheckInRequest, CheckInCommand>();
        }
    }
}
