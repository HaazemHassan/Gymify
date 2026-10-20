using AutoMapper;
using Gymify.Api.Requests.Packages.UpdatePackage;
using Gymify.Application.Features.Packages.Commands.UpdatePackage;

namespace Gymify.API.Requests.Packages.UpdatePackage
{
    public class UpdatePackageRequestMappingProfile : Profile
    {
        public UpdatePackageRequestMappingProfile()
        {
            CreateMap<UpdatePackageRequest, UpdatePackageCommand>();
        }
    }
}
