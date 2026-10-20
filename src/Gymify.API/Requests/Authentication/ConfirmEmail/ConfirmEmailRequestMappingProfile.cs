using AutoMapper;
using Gymify.Application.Features.Authentication.Commands.ConfirmEmail;

namespace Gymify.Api.Requests.Authentication.ConfirmEmail;

public class ConfirmEmailRequestMappingProfile : Profile
{
    public ConfirmEmailRequestMappingProfile()
    {
        CreateMap<ConfirmEmailRequest, ConfirmEmailCommand>();
    }
}
