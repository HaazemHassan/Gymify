using AutoMapper;
using Gymify.Application.Features.Users.Queries.CheckEmailAvailability;

namespace Gymify.Api.Requests.Users.CheckEmailAvailability;

public class CheckEmailAvailabilityRequestMappingProfile : Profile
{
    public CheckEmailAvailabilityRequestMappingProfile()
    {
        CreateMap<CheckEmailAvailabilityRequest, CheckEmailAvailabilityQuery>();
    }
}
