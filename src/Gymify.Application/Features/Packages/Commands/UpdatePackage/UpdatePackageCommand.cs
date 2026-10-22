using Gymify.Application.Common.CQRS;
using Gymify.Application.Security;
using Gymify.Application.Security.Markers;
using Gymify.Domain.Common.Enums;

namespace Gymify.Application.Features.Packages.Commands.UpdatePackage
{
    [Authorize(Permissions = [Permission.PackagesWrite])]
    public class UpdatePackageCommand : ICommand<UpdatePackageCommandResponse>, IAuthorizedRequest
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public decimal? Price { get; set; }
        public int? Duration { get; set; }
        public string? Description { get; set; }
        public bool? IsActive { get; set; }
        public PackageType? Type { get; set; }
        public int? TrainerId { get; set; }
    }
}
