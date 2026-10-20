using AutoMapper;
using Gymify.Application.Features.Authentication.Commands.ChangePassword;

namespace Gymify.Api.Requests.Authentication.ChangePassword;

public class ChangePasswordRequestMappingProfile : Profile
{
    public ChangePasswordRequestMappingProfile()
    {
        CreateMap<ChangePasswordRequest, ChangePasswordCommand>();
    }
}
