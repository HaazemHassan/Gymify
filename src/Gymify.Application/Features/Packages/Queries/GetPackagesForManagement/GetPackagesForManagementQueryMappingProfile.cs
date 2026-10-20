using AutoMapper;
using Gymify.Domain.Entities;

namespace Gymify.Application.Features.Packages.Queries.GetPackagesForManagement
{
    public class GetPackagesForManagementQueryMappingProfile : Profile
    {
        public GetPackagesForManagementQueryMappingProfile()
        {
            CreateMap<Package, GetPackagesForManagementQueryResponse>();
        }
    }
}
