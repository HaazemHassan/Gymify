using PrimeFit.Domain.Common.Enums;
using PrimeFit.Application.Security;
using PrimeFit.Application.Security.Markers;
using PrimeFit.Application.Security.Policies;

namespace PrimeFit.Application.Features.BranchPackages.Commands.UpdatePackageStatus
{
    [Authorize(Policy = AuthorizationPolicies.BranchStaffOnly)]
    [BranchAuthorize(BranchPermissions = [Permission.PackagesWrite])]
    public class UpdatePackageStatusCommand : ICommand, IBranchAuthorizedRequest
    {
        public int BranchId { get; set; }
        public int PackageId { get; set; }
        public bool IsActive { get; set; }

    }
}
