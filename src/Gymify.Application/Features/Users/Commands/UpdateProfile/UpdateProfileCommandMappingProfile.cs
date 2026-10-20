using AutoMapper;
using Gymify.Domain.Entities;

namespace Gymify.Application.Features.Users.Commands.UpdateProfile
{
    public class UpdateProfileCommandMappingProfile : Profile
    {
        public UpdateProfileCommandMappingProfile()
        {
            CreateMap<DomainUser, UpdateProfileCommandResponse>();
        }

    }
}
