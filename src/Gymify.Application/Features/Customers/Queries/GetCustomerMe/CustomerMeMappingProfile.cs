using AutoMapper;
using Gymify.Domain.Entities;

namespace Gymify.Application.Features.Customers.Queries.GetCustomerMe
{
    public class CustomerMeMappingProfile : Profile
    {
        public CustomerMeMappingProfile()
        {
            CreateMap<Customer, GetCustomerMeQueryResponse>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.DomainUserId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.DomainUser != null ? src.DomainUser.Name : string.Empty))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.DomainUser != null ? src.DomainUser.Email : string.Empty))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.DomainUser != null ? src.DomainUser.PhoneNumber : null))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.DomainUser != null ? src.DomainUser.IsActive : false))
                .ForMember(dest => dest.Height, opt => opt.MapFrom(src => src.Height))
                .ForMember(dest => dest.Weight, opt => opt.MapFrom(src => src.Weight))
                .ForMember(dest => dest.ApprovalStatus, opt => opt.MapFrom(src => src.ApprovalStatus));
        }
    }
}
