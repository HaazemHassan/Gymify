using AutoMapper;
using Gymify.Domain.Entities;

namespace Gymify.Application.Features.Lockers.Queries.GetLockers
{
    public class GetLockersQueryMappingProfile : Profile
    {
        public GetLockersQueryMappingProfile()
        {
            CreateMap<Locker, GetLockersQueryResponse>()
                .ForMember(
                    d => d.IsAvailable,
                    opt => opt.MapFrom(s => s.IsActive && !s.Attendances.Any(a => a.CheckOutAt == null)));
        }
    }
}
