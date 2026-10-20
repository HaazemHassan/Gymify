using PrimeFit.Domain.Entities;

namespace PrimeFit.Application.Features.InAppNotifications.Events.InAppNotificationCreated
{
    public record InAppNotificationCreatedEvent(UserNotification Notification) : IPostCommitEvent;
}
