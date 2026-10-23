using AutoMapper;
using Gymify.Domain.Entities;

namespace Gymify.Application.Features.Trainers.Queries.GetTrainerCustomers
{
    public class GetTrainerCustomersQueryMappingProfile : Profile
    {
        public GetTrainerCustomersQueryMappingProfile()
        {
            CreateMap<Subscription, GetTrainerCustomersQueryResponse>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.CustomerId))
                .ForMember(dest => dest.SubscriptionId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.CustomerId, opt => opt.MapFrom(src => src.CustomerId))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Customer.DomainUserId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Customer.DomainUser.Name))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Customer.DomainUser.Email))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.Customer.DomainUser.PhoneNumber))
                .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.Customer.Code))
                .ForMember(dest => dest.ApprovalStatus, opt => opt.MapFrom(src => src.Customer.ApprovalStatus))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.Customer.DomainUser.IsActive))
                .ForMember(dest => dest.PackageName, opt => opt.MapFrom(src => src.PackageName))
                .ForMember(dest => dest.PackagePrice, opt => opt.MapFrom(src => src.PackagePrice))
                .ForMember(dest => dest.PackageDuration, opt => opt.MapFrom(src => src.PackageDuration))
                .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.StartDate))
                .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.EndDate));
        }
    }
}
