using AutoMapper;
using Gymify.Application.Features.Subscriptions.Commands.FreezeSubscription;

namespace Gymify.API.Requests.Subscriptions.FreezeSubscription
{
    public class FreezeSubscriptionRequestMappingProfile : Profile
    {
        public FreezeSubscriptionRequestMappingProfile()
        {
            CreateMap<FreezeSubscriptionRequest, FreezeSubscriptionCommand>();
        }
    }
}
