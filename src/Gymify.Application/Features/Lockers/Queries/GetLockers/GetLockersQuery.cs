using Gymify.Application.Common.CQRS;
using Gymify.Application.Security;
using Gymify.Application.Security.Markers;
using Gymify.Domain.Common.Enums;

namespace Gymify.Application.Features.Lockers.Queries.GetLockers
{
    [Authorize(Permissions = [Permission.LockersRead])]
    public class GetLockersQuery : IQuery<List<GetLockersQueryResponse>>, IAuthorizedRequest
    {
        public bool? IsActive { get; set; }
        public string? Search { get; set; }

        public GetLockersQuery()
        {
        }

        public GetLockersQuery(bool? isActive, string? search = null)
        {
            IsActive = isActive;
            Search = search;
        }
    }
}
