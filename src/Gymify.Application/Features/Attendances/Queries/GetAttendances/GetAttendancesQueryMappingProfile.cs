using AutoMapper;
using Gymify.Domain.Entities;

namespace Gymify.Application.Features.Attendances.Queries.GetAttendances
{
    public class GetAttendancesQueryMappingProfile : Profile
    {
        public GetAttendancesQueryMappingProfile()
        {
            CreateMap<Attendance, GetAttendancesQueryResponse>()
                .ForMember(d => d.Id, opt => opt.MapFrom(s => s.Id))
                .ForMember(d => d.CustomerId, opt => opt.MapFrom(s => s.CustomerId))
                .ForMember(d => d.CustomerName, opt => opt.MapFrom(s => s.Customer.DomainUser.Name))
                .ForMember(d => d.CustomerEmail, opt => opt.MapFrom(s => s.Customer.DomainUser.Email))
                .ForMember(d => d.CustomerPhoneNumber, opt => opt.MapFrom(s => s.Customer.DomainUser.PhoneNumber))
                .ForMember(d => d.AttendanceCode, opt => opt.MapFrom(s => s.Code))
                .ForMember(d => d.LockerId, opt => opt.MapFrom(s => s.LockerId))
                .ForMember(d => d.LockerNumber, opt => opt.MapFrom(s => s.Locker.Number))
                .ForMember(d => d.CheckInAt, opt => opt.MapFrom(s => s.CheckInAt))
                .ForMember(d => d.CheckOutAt, opt => opt.MapFrom(s => s.CheckOutAt))
                .ForMember(d => d.IsCurrentlyAttending, opt => opt.MapFrom(s => s.CheckOutAt == null));
        }
    }
}
