using AutoMapper;
using Gymify.Application.Features.Users.Commands.UpdateProfile;

namespace Gymify.Api.Requests.Users.UpdateProfile {
    public class UpdateProfileRequestMappingProfile : Profile
    {
        public UpdateProfileRequestMappingProfile()
        {
            CreateMap<UpdateProfileRequest, UpdateProfileCommand>();
        }
    }
}
