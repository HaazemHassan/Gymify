using AutoMapper;
using Gymify.Domain.Entities;

namespace Gymify.Application.Features.Packages.Queries.GetPackageByIdForManagement
{
    public class GetPackageByIdForManagementQueryMappingProfile : Profile
    {
        public GetPackageByIdForManagementQueryMappingProfile()
        {
            CreateMap<Package, GetPackageByIdForManagementQueryResponse>();
        }
    }
}
