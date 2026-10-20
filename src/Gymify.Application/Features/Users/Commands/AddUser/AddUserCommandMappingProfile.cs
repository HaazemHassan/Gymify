using AutoMapper;
using Gymify.Domain.Entities;

namespace Gymify.Application.Features.Users.Commands.AddUser
{
    public class AddUserCommandMappingProfile : Profile
    {
        public AddUserCommandMappingProfile()
        {
            CreateMap<DomainUser, AddUserCommandResponse>();
        }

    }
}
