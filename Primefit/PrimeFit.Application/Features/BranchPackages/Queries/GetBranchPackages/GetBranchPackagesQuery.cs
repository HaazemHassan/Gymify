using PrimeFit.Domain.Common.Enums;
using PrimeFit.Application.Common.Pagination;
using PrimeFit.Application.Security;
using PrimeFit.Application.Security.Markers;
using PrimeFit.Application.Security.Policies;

namespace PrimeFit.Application.Features.BranchPackages.Queries.GetBranchPackages
{

    [Authorize(Policy = AuthorizationPolicies.BranchStaffOnly)]
    [BranchAuthorize(BranchPermissions = [Permission.PackagesView])]
    public class GetBranchPackagesQuery : PaginatedQuery
        , IQuery<PaginatedResult<GetBranchPackagesQueryResponse>>, IBranchAuthorizedRequest
    {

        public int BranchId { get; set; }


    }
}
