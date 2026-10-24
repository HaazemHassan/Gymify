using AutoMapper;
using Gymify.Application.Features.Subscriptions.Commands.CancelSubscription;

namespace Gymify.API.Requests.Subscriptions.CancelSubscription
{
    public class CancelSubscriptionRequestMappingProfile : Profile
    {
        public CancelSubscriptionRequestMappingProfile()
        {
            CreateMap<CancelSubscriptionRequest, CancelSubscriptionCommand>();
        }
    }
}
