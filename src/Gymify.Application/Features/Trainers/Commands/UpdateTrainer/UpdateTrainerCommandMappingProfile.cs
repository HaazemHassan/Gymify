using AutoMapper;
using Gymify.Domain.Entities;

namespace Gymify.Application.Features.Trainers.Commands.UpdateTrainer
{
    public class UpdateTrainerCommandMappingProfile : Profile
    {
        public UpdateTrainerCommandMappingProfile()
        {
            CreateMap<Trainer, UpdateTrainerCommandResponse>()
                .ForMember(d => d.Id, opt => opt.MapFrom(s => s.Id))
                .ForMember(d => d.TrainerId, opt => opt.MapFrom(s => s.Id))
                .ForMember(d => d.EmployeeId, opt => opt.MapFrom(s => s.EmployeeId))
                .ForMember(d => d.UserId, opt => opt.MapFrom(s => s.Employee.DomainUserId))
                .ForMember(d => d.Name, opt => opt.MapFrom(s => s.Employee.DomainUser.Name))
                .ForMember(d => d.Email, opt => opt.MapFrom(s => s.Employee.DomainUser.Email))
                .ForMember(d => d.PhoneNumber, opt => opt.MapFrom(s => s.Employee.DomainUser.PhoneNumber))
                .ForMember(d => d.YearsOfExperience, opt => opt.MapFrom(s => s.YearsOfExperience))
                .ForMember(d => d.Salary, opt => opt.MapFrom(s => s.Employee.Salary))
                .ForMember(d => d.IsTrainerActive, opt => opt.MapFrom(s => s.IsActive));
        }
    }
}
