using Gymify.Application.Common.CQRS;
using Gymify.Application.Common.Pagination;
using Gymify.Application.Security;
using Gymify.Application.Security.Markers;
using Gymify.Domain.Common.Enums;

namespace Gymify.Application.Features.Trainers.Queries.GetTrainersForManagement
{
    [Authorize(Permissions = [Permission.TrainersRead])]
    public class GetTrainersForManagementQuery : PaginatedQuery, IQuery<PaginatedResult<GetTrainersForManagementQueryResponse>>, IAuthorizedRequest
    {
        public bool? IsActive { get; set; }
    }
}
