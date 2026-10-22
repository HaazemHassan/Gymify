using AutoMapper;
using Gymify.Domain.Entities;

namespace Gymify.Application.Features.Subscriptions.Commands.CreateSubscription
{
    public class CreateSubscriptionCommandMappingProfile : Profile
    {
        public CreateSubscriptionCommandMappingProfile()
        {
            CreateMap<Subscription, CreateSubscriptionCommandResponse>();
        }
    }
}
