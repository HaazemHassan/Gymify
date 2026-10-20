using AutoMapper;
using Gymify.Application.Features.Authentication.Commands.SignIn;

namespace Gymify.Api.Requests.Authentication.SignInWithPassword;

public class SignInWithPasswordRequestMappingProfile : Profile
{
    public SignInWithPasswordRequestMappingProfile()
    {
        CreateMap<SignInWithPasswordRequest, SignInWithPasswordCommand>();
    }
}
