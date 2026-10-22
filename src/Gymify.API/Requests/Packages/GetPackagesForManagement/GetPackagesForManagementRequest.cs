using Gymify.Api.Requests.Common.Pagination;
using Gymify.Domain.Common.Enums;

namespace Gymify.API.Requests.Packages.GetPackagesForManagement
{
    public class GetPackagesForManagementRequest : BasicPaginationRequest
    {
        public List<PackageType>? Types { get; set; }
        public int? TrainerId { get; set; }
        public bool? IsActive { get; set; }
    }
}
