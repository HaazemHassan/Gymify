using PrimeFit.Application.Security;
using PrimeFit.Application.Security.Markers;

namespace PrimeFit.Application.Features.InAppNotifications.Commands.MarkNotificationAsRead
{
    [Authorize]
    public class MarkNotificationAsReadCommand : ICommand, IAuthorizedRequest
    {
        public int NotificationId { get; set; }
    }
}
