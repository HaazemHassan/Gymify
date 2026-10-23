using Gymify.Application.Common.CQRS;
using Gymify.Application.Common.Pagination;
using Gymify.Application.Security;
using Gymify.Application.Security.Markers;
using Gymify.Domain.Common.Enums;

namespace Gymify.Application.Features.Kpis.Queries.GetKpisPaginated
{
    [Authorize(Permissions = [Permission.TrainersRead])]
    public class GetKpisPaginatedQuery : PaginatedQuery, IQuery<PaginatedResult<GetKpisPaginatedQueryResponse>>, IAuthorizedRequest
    {
        public bool? IsActive { get; set; }
    }
}
