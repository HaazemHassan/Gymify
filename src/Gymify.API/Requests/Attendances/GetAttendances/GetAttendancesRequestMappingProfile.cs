using AutoMapper;
using Gymify.Application.Features.Attendances.Queries.GetAttendances;

namespace Gymify.API.Requests.Attendances.GetAttendances
{
    public class GetAttendancesRequestMappingProfile : Profile
    {
        public GetAttendancesRequestMappingProfile()
        {
            CreateMap<GetAttendancesRequest, GetAttendancesQuery>()
                .ForMember(d => d.PageNumber, opt => opt.MapFrom(s => s.PageNumber ?? 1))
                .ForMember(d => d.PageSize, opt => opt.MapFrom(s => s.PageSize ?? 10))
                .ForMember(d => d.Search, opt => opt.MapFrom(s => s.Search))
                .ForMember(d => d.IsCurrentlyAttending, opt => opt.MapFrom(s => s.IsCurrentlyAttending))
                .ForMember(d => d.FromDate, opt => opt.MapFrom(s => s.FromDate));
        }
    }
}
