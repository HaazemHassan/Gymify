using AutoMapper;
using Gymify.Application.Features.Subscriptions.Queries.GetSubscriptionsForManagement;

namespace Gymify.API.Requests.Subscriptions.GetSubscriptionsForManagement
{
    public class GetSubscriptionsForManagementRequestMappingProfile : Profile
    {
        public GetSubscriptionsForManagementRequestMappingProfile()
        {
            CreateMap<GetSubscriptionsForManagementRequest, GetSubscriptionsForManagementQuery>();
        }
    }
}
