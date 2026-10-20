using AutoMapper;
using Gymify.Application.Features.Subscriptions.Commands.CreateSubscription;

namespace Gymify.API.Requests.Subscriptions.CreateSubscription
{
    public class CreateSubscriptionRequestMappingProfile : Profile
    {
        public CreateSubscriptionRequestMappingProfile()
        {
            CreateMap<CreateSubscriptionRequest, CreateSubscriptionCommand>();
        }
    }
}
