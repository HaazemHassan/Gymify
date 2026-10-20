using AutoMapper;
using Gymify.Application.Features.Authentication.Commands.SendResetPasswordEmail;

namespace Gymify.Api.Requests.Authentication.SendResetPasswordEmail;

public class SendResetPasswordEmailRequestMappingProfile : Profile
{
    public SendResetPasswordEmailRequestMappingProfile()
    {
        CreateMap<SendResetPasswordEmailRequest, SendResetPasswordEmailCommand>();
    }
}
