using AutoMapper;
using Gymify.Domain.Entities;

namespace Gymify.Application.Features.Attendances.Queries.GetCustomerAttendances
{
    public class GetCustomerAttendancesQueryMappingProfile : Profile
    {
        public GetCustomerAttendancesQueryMappingProfile()
        {
            CreateMap<Attendance, CustomerAttendanceItemResponse>()
                .ForMember(d => d.Id, opt => opt.MapFrom(s => s.Id))
                .ForMember(d => d.CustomerId, opt => opt.MapFrom(s => s.CustomerId))
                .ForMember(d => d.CheckInAt, opt => opt.MapFrom(s => s.CheckInAt))
                .ForMember(d => d.CheckOutAt, opt => opt.MapFrom(s => s.CheckOutAt))
                .ForMember(d => d.LockerId, opt => opt.MapFrom(s => s.LockerId))
                .ForMember(d => d.LockerNumber, opt => opt.MapFrom(s => s.Locker.Number))
                .ForMember(d => d.AttendanceCode, opt => opt.MapFrom(s => s.Code))
                .ForMember(d => d.IsCurrentlyAttending, opt => opt.MapFrom(s => s.CheckOutAt == null))
                .ForMember(d => d.Duration, opt => opt.MapFrom(s => s.CheckOutAt.HasValue ? s.CheckOutAt.Value - s.CheckInAt : (TimeSpan?)null));
        }
    }
}
