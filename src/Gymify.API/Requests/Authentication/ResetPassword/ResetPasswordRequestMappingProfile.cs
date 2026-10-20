using AutoMapper;
using Gymify.Application.Features.Authentication.Commands.ResetPassword;

namespace Gymify.Api.Requests.Authentication.ResetPassword;

public class ResetPasswordRequestMappingProfile : Profile
{
    public ResetPasswordRequestMappingProfile()
    {
        CreateMap<ResetPasswordRequest, ResetPasswordCommand>();
    }
}
