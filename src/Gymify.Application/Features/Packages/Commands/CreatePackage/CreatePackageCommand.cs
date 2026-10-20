using Gymify.Application.Common.CQRS;
using Gymify.Application.Security;
using Gymify.Application.Security.Markers;
using Gymify.Domain.Common.Enums;

namespace Gymify.Application.Features.Packages.Commands.CreatePackage
{
    [Authorize(Permissions = [Permission.PackagesWrite])]
    public class CreatePackageCommand : ICommand<CreatePackageCommandResponse>, IAuthorizedRequest
    {
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Duration { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
