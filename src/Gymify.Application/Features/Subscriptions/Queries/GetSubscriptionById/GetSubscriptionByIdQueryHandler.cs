using Gymify.Application.Common.CQRS;
using Gymify.Application.Features.Subscriptions.Queries.GetSubscriptionById.Specifications;
using Gymify.Application.ServicesContracts.Infrastructure;
using Gymify.Domain.Common.Constants;
using Gymify.Domain.RepositoriesContracts;

namespace Gymify.Application.Features.Subscriptions.Queries.GetSubscriptionById
{
    public class GetSubscriptionByIdQueryHandler : IQueryHandler<GetSubscriptionByIdQuery, GetSubscriptionByIdQueryResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDateTimeProvider _dateTimeProvider;

        public GetSubscriptionByIdQueryHandler(
            IUnitOfWork unitOfWork,
            IDateTimeProvider dateTimeProvider)
        {
            _unitOfWork = unitOfWork;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<ErrorOr<GetSubscriptionByIdQueryResponse>> Handle(
            GetSubscriptionByIdQuery request,
            CancellationToken cancellationToken)
        {
            var now = _dateTimeProvider.UtcNow;

            var spec = new SubscriptionDetailsByIdSpec(request.Id, now);
            var subscription = await _unitOfWork.Subscriptions.FirstOrDefaultAsync(spec, cancellationToken);

            if (subscription is null)
            {
                return Error.NotFound(
                    code: ErrorCodes.Subscription.SubscriptionNotFound,
                    description: $"Subscription with ID {request.Id} was not found.");
            }

            return subscription;
        }
    }
}
