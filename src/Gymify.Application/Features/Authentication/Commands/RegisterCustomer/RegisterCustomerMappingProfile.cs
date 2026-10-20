using AutoMapper;
using Gymify.Domain.Common.Enums;
using Gymify.Domain.Entities;

namespace Gymify.Application.Features.Authentication.Commands.RegisterCustomer
{
    public class RegisterCustomerMappingProfile : Profile
    {
        public RegisterCustomerMappingProfile()
        {
            CreateMap<DomainUser, RegisterCustomerResponse>()
                .ForMember(dest => dest.ApprovalStatus, opt => opt.MapFrom(src => src.Customer != null ? src.Customer.ApprovalStatus : ApprovalStatus.Pending));
        }
    }
}
