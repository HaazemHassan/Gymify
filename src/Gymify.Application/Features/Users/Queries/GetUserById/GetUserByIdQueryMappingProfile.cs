using AutoMapper;
using Gymify.Domain.Entities;

namespace Gymify.Application.Features.Users.Queries.GetUserById
{
    public class GetUserByIdQueryMappingProfile : Profile
    {
        public GetUserByIdQueryMappingProfile()
        {
            CreateMap<DomainUser, GetUserByIdQueryResponse>();
        }

    }
}