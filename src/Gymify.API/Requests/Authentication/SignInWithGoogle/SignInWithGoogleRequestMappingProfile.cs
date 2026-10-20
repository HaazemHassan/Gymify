using AutoMapper;
using Gymify.Application.Features.Authentication.Commands.SignInWithGoogle;

namespace Gymify.Api.Requests.Authentication.SignInWithGoogle;

public class SignInWithGoogleRequestMappingProfile : Profile
{
    public SignInWithGoogleRequestMappingProfile()
    {
        CreateMap<SignInWithGoogleRequest, SignInWithGoogleCommand>();
    }
}
