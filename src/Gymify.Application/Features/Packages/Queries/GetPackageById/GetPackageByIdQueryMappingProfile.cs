using AutoMapper;
using Gymify.Domain.Entities;

namespace Gymify.Application.Features.Packages.Queries.GetPackageById
{
    public class GetPackageByIdQueryMappingProfile : Profile
    {
        public GetPackageByIdQueryMappingProfile()
        {
            CreateMap<Package, GetPackageByIdQueryResponse>();
        }
    }
}
