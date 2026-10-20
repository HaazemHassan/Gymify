using AutoMapper;
using Gymify.Domain.Entities;

namespace Gymify.Application.Features.Customers.Commands.UpdateCustomer
{
    public class UpdateCustomerCommandMappingProfile : Profile
    {
        public UpdateCustomerCommandMappingProfile()
        {
            CreateMap<Customer, UpdateCustomerCommandResponse>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.CustomerId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.DomainUserId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.DomainUser != null ? src.DomainUser.Name : string.Empty))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.DomainUser != null ? src.DomainUser.Email : string.Empty))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.DomainUser != null ? src.DomainUser.PhoneNumber : null))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.DomainUser != null ? src.DomainUser.IsActive : false));
        }
    }
}
