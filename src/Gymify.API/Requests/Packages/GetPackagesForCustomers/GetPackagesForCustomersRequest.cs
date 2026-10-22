using Gymify.Api.Requests.Common.Pagination;
using Gymify.Domain.Common.Enums;

namespace Gymify.API.Requests.Packages.GetPackagesForCustomers
{
    public class GetPackagesForCustomersRequest : BasicPaginationRequest
    {
        public List<PackageType>? Types { get; set; }
    }
}
