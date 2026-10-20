using AutoMapper;
using Gymify.Application.Features.Authentication.Commands.ResendConfirmEmail;

namespace Gymify.Api.Requests.Authentication.ResendConfirmationEmail;

public class ResendConfirmationEmailRequestMappingProfile : Profile
{
    public ResendConfirmationEmailRequestMappingProfile()
    {
        CreateMap<ResendConfirmationEmailRequest, ResendConfirmationEmailCommand>();
    }
}
