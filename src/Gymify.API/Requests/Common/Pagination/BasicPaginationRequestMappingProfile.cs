using AutoMapper;
using Gymify.Application.Features.Trainers.Queries.GetTrainersForCustomers;

namespace Gymify.Api.Requests.Common.Pagination
{
    public class BasicPaginationRequestMappingProfile : Profile
    {
        public BasicPaginationRequestMappingProfile()
        {
            CreateMap<BasicPaginationRequest, GetTrainersForCustomersQuery>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember is not null));
        }
    }
}
