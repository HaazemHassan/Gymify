using AutoMapper;
using Gymify.Application.Features.Authentication.Commands.Logout;

namespace Gymify.Api.Requests.Authentication.Logout;

public class LogoutRequestMappingProfile : Profile
{
    public LogoutRequestMappingProfile()
    {
        CreateMap<LogoutRequest, LogoutCommand>();
    }
}
