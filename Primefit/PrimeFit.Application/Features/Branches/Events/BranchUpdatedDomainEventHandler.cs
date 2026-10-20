using PrimeFit.Application.Contracts.Api;
using PrimeFit.Application.Features.InAppNotifications.Events.InAppNotificationCreated;
using PrimeFit.Application.ServicesContracts.Infrastructure;
using PrimeFit.Domain.Common.Enums;
using PrimeFit.Domain.DomainEvents;

namespace PrimeFit.Application.Features.Branches.Events
{
    /// <summary>
    /// Adds an in-app notification and triggers a real-time event when a branch's details are updated by a staff member.
    /// </summary>
    public class BranchUpdatedDomainEventHandler : INotificationHandler<BranchUpdatedDomainEvent>
    {
        private readonly INotificationHelperService _notificationHelper;
        private readonly IInMemoryEventDispatcher _eventDispatcher;
        private readonly ICurrentUserService _currentUserService;

        public BranchUpdatedDomainEventHandler(
            INotificationHelperService notificationHelper,
            IInMemoryEventDispatcher eventDispatcher,
            ICurrentUserService currentUserService)
        {
            _notificationHelper = notificationHelper;
            _eventDispatcher = eventDispatcher;
            _currentUserService = currentUserService;
        }

        public async Task Handle(BranchUpdatedDomainEvent notificationEvent, CancellationToken cancellationToken)
        {
            if (!_currentUserService.UserId.HasValue)
            {
                return;
            }
            int curUserId = _currentUserService.UserId.Value;
            if (curUserId == notificationEvent.OwnerId)
            {
                return;
            }

            var notification = await _notificationHelper.AddNotificationAsync(
                userId: notificationEvent.OwnerId,
                title: "Branch Details Updated",
                message: $"The business details of branch '{notificationEvent.BranchName}' have been updated by a staff member.",
                notificationType: NotificationType.BranchUpdate,
                ct: cancellationToken);

            _eventDispatcher.EnqueueEvent(new InAppNotificationCreatedEvent(notification));
        }
    }
}
