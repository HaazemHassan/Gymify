using Gymify.Application.Features.Subscriptions.Specifications;
using Gymify.Application.ServicesContracts.Infrastructure;
using Gymify.Domain.Common.Constants;
using Gymify.Domain.RepositoriesContracts;

namespace Gymify.Application.Features.Subscriptions.Commands.FreezeSubscription
{
    public class FreezeSubscriptionCommandHandler : ICommandHandler<FreezeSubscriptionCommand, FreezeSubscriptionCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDateTimeProvider _dateTimeProvider;

        public FreezeSubscriptionCommandHandler(
            IUnitOfWork unitOfWork,
            IDateTimeProvider dateTimeProvider)
        {
            _unitOfWork = unitOfWork;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<ErrorOr<FreezeSubscriptionCommandResponse>> Handle(
            FreezeSubscriptionCommand request,
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

            var freezeResult = subscription.Freeze(now, request.EndDate, request.Reason);
            if (freezeResult.IsError)
            {
                return freezeResult.Errors;
            }

            var freeze = freezeResult.Value;

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new FreezeSubscriptionCommandResponse
            {
                FreezeId = freeze.Id,
                SubscriptionId = subscription.Id,
                FreezeStartDate = freeze.StartDate,
                FreezeEndDate = freeze.EndDate,
                NewSubscriptionEndDate = subscription.EndDate,
                Reason = freeze.Reason
            };
        }
    }
}
