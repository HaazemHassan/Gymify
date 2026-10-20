using AutoMapper;
using PrimeFit.Application.Common.DTOS;
using PrimeFit.Application.ServicesContracts.Infrastructure;

namespace PrimeFit.Application.Features.InAppNotifications.Events.InAppNotificationCreated
{
    public class InAppNotificationCreatedEventHandler : INotificationHandler<InAppNotificationCreatedEvent>
    {
        private readonly INotificationHelperService _notificationHelper;
        private readonly IMapper _mapper;

        public InAppNotificationCreatedEventHandler(INotificationHelperService notificationHelper, IMapper mapper)
        {
            _notificationHelper = notificationHelper;
            _mapper = mapper;
        }

        public async Task Handle(InAppNotificationCreatedEvent notificationEvent, CancellationToken cancellationToken)
        {
            var dto = _mapper.Map<InAppNotificationDto>(notificationEvent.Notification);
            await _notificationHelper.PushRealTimeAsync(notificationEvent.Notification.UserId, dto);
        }
    }
}
