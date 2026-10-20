using AutoMapper;
using Gymify.Domain.Entities;

namespace Gymify.Application.Features.Users.Queries.GetUsersPaginated
{
    public class GetUsersPaginatedQueryMappingProfile : Profile
    {
        public GetUsersPaginatedQueryMappingProfile()
        {
            CreateMap<DomainUser, GetUsersPaginatedQueryResponse>()
           .ForMember(dest => dest.Phone,
              opt => opt.MapFrom(src => src.PhoneNumber));
        }

    }
}


