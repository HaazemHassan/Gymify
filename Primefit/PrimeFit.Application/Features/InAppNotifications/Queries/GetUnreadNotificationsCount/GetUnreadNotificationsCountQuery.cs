using PrimeFit.Application.Security;
using PrimeFit.Application.Security.Markers;

namespace PrimeFit.Application.Features.InAppNotifications.Queries.GetUnreadNotificationsCount
{
    [Authorize]
    public class GetUnreadNotificationsCountQuery
        : IQuery<int>, IAuthorizedRequest
    {
    }
}
