using AutoMapper;
using Gymify.Domain.Entities;

namespace Gymify.Application.Features.CustomerBehaviorNotes.Queries.GetCustomerBehaviorNotes
{
    public class GetCustomerBehaviorNotesQueryMappingProfile : Profile
    {
        public GetCustomerBehaviorNotesQueryMappingProfile()
        {
            CreateMap<CustomerBehaviorNote, GetCustomerBehaviorNotesQueryResponse>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.CustomerId, opt => opt.MapFrom(src => src.CustomerId))
                .ForMember(dest => dest.EmployeeId, opt => opt.MapFrom(src => src.EmployeeId))
                .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.Customer.DomainUser.Name))
                .ForMember(dest => dest.EmployeeName, opt => opt.MapFrom(src => src.Employee.DomainUser.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt));
        }
    }
}
