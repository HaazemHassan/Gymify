using Gymify.Application.Common.CQRS;
using Gymify.Application.Security;
using Gymify.Application.Security.Markers;
using Gymify.Domain.Common.Enums;

namespace Gymify.Application.Features.Packages.Queries.GetPackageByIdForManagement
{
    [Authorize(Permissions = [Permission.PackagesRead])]
    public class GetPackageByIdForManagementQuery : IQuery<GetPackageByIdForManagementQueryResponse>, IAuthorizedRequest
    {
        public int Id { get; set; }

        public GetPackageByIdForManagementQuery()
        {
        }

        public GetPackageByIdForManagementQuery(int id)
        {
            Id = id;
        }
    }
}
