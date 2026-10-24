using AutoMapper;
using Gymify.Application.Features.Subscriptions.Queries.GetCustomerSubscriptions;

namespace Gymify.API.Requests.Customers.GetCustomerSubscriptions
{
    public class GetCustomerSubscriptionsRequestMappingProfile : Profile
    {
        public GetCustomerSubscriptionsRequestMappingProfile()
        {
            CreateMap<GetCustomerSubscriptionsRequest, GetCustomerSubscriptionsQuery>();
        }
    }
}
