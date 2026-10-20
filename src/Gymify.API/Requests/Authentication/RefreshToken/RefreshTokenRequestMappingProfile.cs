using AutoMapper;
using Gymify.Application.Features.Authentication.Commands.RefreshToken;

namespace Gymify.Api.Requests.Authentication.RefreshToken;

public class RefreshTokenRequestMappingProfile : Profile
{
    public RefreshTokenRequestMappingProfile()
    {
        CreateMap<RefreshTokenRequest, RefreshTokenCommand>();
    }
}
