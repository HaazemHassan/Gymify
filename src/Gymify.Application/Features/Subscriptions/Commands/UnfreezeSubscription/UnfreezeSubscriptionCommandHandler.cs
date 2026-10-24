using Gymify.Application.Common.CQRS;
using Gymify.Application.Features.Subscriptions.Specifications;
using Gymify.Application.ServicesContracts.Infrastructure;
using Gymify.Domain.Common.Constants;
using Gymify.Domain.RepositoriesContracts;

namespace Gymify.Application.Features.Subscriptions.Commands.UnfreezeSubscription
{
    public class UnfreezeSubscriptionCommandHandler : ICommandHandler<UnfreezeSubscriptionCommand, UnfreezeSubscriptionCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDateTimeProvider _dateTimeProvider;

        public UnfreezeSubscriptionCommandHandler(
            IUnitOfWork unitOfWork,
            IDateTimeProvider dateTimeProvider)
        {
            _unitOfWork = unitOfWork;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<ErrorOr<UnfreezeSubscriptionCommandResponse>> Handle(
            UnfreezeSubscriptionCommand request,
            CancellationToken cancellationToken)
        {
            var now = _dateTimeProvider.UtcNow;

            var subscription = await _unitOfWork.Subscriptions.FirstOrDefaultAsync(
                new SubscriptionWithFreezesByIdSpec(request.SubscriptionId),
                cancellationToken);

            if (subscription is null)
            {
                return Error.NotFound(
                    code: ErrorCodes.Subscription.SubscriptionNotFound,
                    description: $"Subscription with ID {request.SubscriptionId} was not found.");
            }

            var unfreezeResult = subscription.Unfreeze(now);
            if (unfreezeResult.IsError)
            {
                return unfreezeResult.Errors;
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new UnfreezeSubscriptionCommandResponse
            {
                SubscriptionId = subscription.Id,
                NewSubscriptionEndDate = subscription.EndDate
            };
        }
    }
}
