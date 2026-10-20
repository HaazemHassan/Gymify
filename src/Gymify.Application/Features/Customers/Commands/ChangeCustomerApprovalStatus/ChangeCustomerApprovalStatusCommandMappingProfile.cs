using AutoMapper;
using Gymify.Domain.Entities;

namespace Gymify.Application.Features.Customers.Commands.ChangeCustomerApprovalStatus
{
    public class ChangeCustomerApprovalStatusCommandMappingProfile : Profile
    {
        public ChangeCustomerApprovalStatusCommandMappingProfile()
        {
            CreateMap<Customer, ChangeCustomerApprovalStatusCommandResponse>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.CustomerId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.DomainUserId))
                .ForMember(dest => dest.ApprovalStatus, opt => opt.MapFrom(src => src.ApprovalStatus));
        }
    }
}
