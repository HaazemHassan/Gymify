using ErrorOr;
using Gymify.Application.Common.CQRS;
using Gymify.Application.ServicesContracts.Infrastructure;
using Gymify.Domain.Common.Constants;
using Gymify.Domain.RepositoriesContracts;

namespace Gymify.Application.Features.Subscriptions.Commands.CancelSubscription
{
    public class CancelSubscriptionCommandHandler : ICommandHandler<CancelSubscriptionCommand, CancelSubscriptionCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDateTimeProvider _dateTimeProvider;

        public CancelSubscriptionCommandHandler(
            IUnitOfWork unitOfWork,
            IDateTimeProvider dateTimeProvider)
        {
            _unitOfWork = unitOfWork;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<ErrorOr<CancelSubscriptionCommandResponse>> Handle(
            CancelSubscriptionCommand request,
            CancellationToken cancellationToken)
        {
            var now = _dateTimeProvider.UtcNow;

            var subscription = await _unitOfWork.Subscriptions.GetByIdAsync(
                request.SubscriptionId,
                cancellationToken);

            if (subscription is null)
            {
                return Error.NotFound(
                    code: ErrorCodes.Subscription.SubscriptionNotFound,
                    description: $"Subscription with ID {request.SubscriptionId} was not found.");
            }

            var cancelResult = subscription.Cancel(now, request.Reason);
            if (cancelResult.IsError)
            {
                return cancelResult.Errors;
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new CancelSubscriptionCommandResponse
            {
                SubscriptionId = subscription.Id,
                CancelledAt = now,
                CancellationReason = subscription.CancellationReason
            };
        }
    }
}
