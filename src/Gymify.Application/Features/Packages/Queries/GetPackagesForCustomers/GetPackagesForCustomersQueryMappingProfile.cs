using AutoMapper;
using Gymify.Domain.Entities;

namespace Gymify.Application.Features.Packages.Queries.GetPackagesForCustomers
{
    public class GetPackagesForCustomersQueryMappingProfile : Profile
    {
        public GetPackagesForCustomersQueryMappingProfile()
        {
            CreateMap<Package, GetPackagesForCustomersQueryResponse>();
        }
    }
}
