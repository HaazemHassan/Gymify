using AutoMapper;
using Gymify.Domain.Entities;

namespace Gymify.Application.Features.Customers.Queries.GetCustomers
{
    public class GetCustomersQueryMappingProfile : Profile
    {
        public GetCustomersQueryMappingProfile()
        {
            CreateMap<Customer, GetCustomersQueryResponse>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.CustomerId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.DomainUserId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.DomainUser != null ? src.DomainUser.Name : string.Empty))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.DomainUser != null ? src.DomainUser.Email : string.Empty))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.DomainUser != null ? src.DomainUser.PhoneNumber : null))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.DomainUser != null ? src.DomainUser.IsActive : false))
                .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.Code))
                .ForMember(dest => dest.Height, opt => opt.MapFrom(src => src.Height))
                .ForMember(dest => dest.Weight, opt => opt.MapFrom(src => src.Weight))
                .ForMember(dest => dest.Age, opt => opt.MapFrom(src => src.Age))
                .ForMember(dest => dest.ApprovalStatus, opt => opt.MapFrom(src => src.ApprovalStatus))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt));
        }
    }
}
