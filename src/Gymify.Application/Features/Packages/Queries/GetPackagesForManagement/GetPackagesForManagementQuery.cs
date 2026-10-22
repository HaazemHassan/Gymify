using Gymify.Application.Common.CQRS;
using Gymify.Application.Common.Pagination;
using Gymify.Application.Security;
using Gymify.Application.Security.Markers;
using Gymify.Domain.Common.Enums;

namespace Gymify.Application.Features.Packages.Queries.GetPackagesForManagement
{
    [Authorize(Permissions = [Permission.PackagesRead])]
    public class GetPackagesForManagementQuery : PaginatedQuery, IQuery<PaginatedResult<GetPackagesForManagementQueryResponse>>, IAuthorizedRequest
    {
        public List<PackageType>? Types { get; set; }
        public int? TrainerId { get; set; }
        public bool? IsActive { get; set; }
    }
}
