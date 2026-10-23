using Gymify.Api.Requests.Common.Pagination;

namespace Gymify.API.Requests.Kpis.GetKpisPaginated
{
    public class GetKpisPaginatedRequest : BasicPaginationRequest
    {
        public bool? IsActive { get; set; }
    }
}
