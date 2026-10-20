using AutoMapper;
using Gymify.Domain.Entities;

namespace Gymify.Application.Features.Trainers.Queries.GetTrainersForCustomers
{
    public class GetTrainersForCustomersQueryMappingProfile : Profile
    {
        public GetTrainersForCustomersQueryMappingProfile()
        {
            CreateMap<Trainer, GetTrainersForCustomersQueryResponse>()
                .ForMember(d => d.Id, opt => opt.MapFrom(s => s.Id))
                .ForMember(d => d.TrainerId, opt => opt.MapFrom(s => s.Id))
                .ForMember(d => d.EmployeeId, opt => opt.MapFrom(s => s.EmployeeId))
                .ForMember(d => d.UserId, opt => opt.MapFrom(s => s.Employee.DomainUserId))
                .ForMember(d => d.Name, opt => opt.MapFrom(s => s.Employee.DomainUser.Name))
                .ForMember(d => d.Email, opt => opt.MapFrom(s => s.Employee.DomainUser.Email))
                .ForMember(d => d.PhoneNumber, opt => opt.MapFrom(s => s.Employee.DomainUser.PhoneNumber))
                .ForMember(d => d.YearsOfExperience, opt => opt.MapFrom(s => s.YearsOfExperience));
        }
    }
}
