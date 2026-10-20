using AutoMapper;
using Gymify.Application.Features.Users.Common;
using Gymify.Domain.Entities;

namespace Gymify.Application.Features.Users.Mapping;

public partial class UserBaseResponseMappingProfile : Profile
{
    public UserBaseResponseMappingProfile()
    {
        CreateMap<DomainUser, UserBaseResponse>();
    }
}
