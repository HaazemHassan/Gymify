using AutoMapper;
using Gymify.Api.Requests.Packages.CreatePackage;
using Gymify.Application.Features.Packages.Commands.CreatePackage;

namespace Gymify.API.Requests.Packages.CreatePackage
{
    public class CreatePackageRequestMappingProfile : Profile
    {
        public CreatePackageRequestMappingProfile()
        {
            CreateMap<CreatePackageRequest, CreatePackageCommand>();
        }
    }
}
