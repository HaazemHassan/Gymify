using AutoMapper;
using Gymify.Api.Requests.Common.Pagination;
using Gymify.Application.Features.Packages.Queries.GetPackagesForCustomers;
using Gymify.Application.Features.Packages.Queries.GetPackagesForManagement;

namespace Gymify.Api.Requests.Common.Pagination
{
    public class BasicPaginationRequestMappingProfile : Profile
    {
        public BasicPaginationRequestMappingProfile()
        {
            CreateMap<BasicPaginationRequest, GetPackagesForCustomersQuery>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember is not null));

            CreateMap<BasicPaginationRequest, GetPackagesForManagementQuery>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember is not null));
        }
    }
}
