using ErrorOr;
using Gymify.Application.Common.CQRS;
using Gymify.Application.Common.Pagination;
using Gymify.Application.Features.Subscriptions.Specifications;
using Gymify.Application.ServicesContracts.Infrastructure;
using Gymify.Domain.RepositoriesContracts;

namespace Gymify.Application.Features.Subscriptions.Queries.GetSubscriptionsForManagement
{
    public class GetSubscriptionsForManagementQueryHandler
        : IQueryHandler<GetSubscriptionsForManagementQuery, PaginatedResult<GetSubscriptionsForManagementQueryResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDateTimeProvider _dateTimeProvider;

        public GetSubscriptionsForManagementQueryHandler(
            IUnitOfWork unitOfWork,
            IDateTimeProvider dateTimeProvider)
        {
            _unitOfWork = unitOfWork;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<ErrorOr<PaginatedResult<GetSubscriptionsForManagementQueryResponse>>> Handle(
            GetSubscriptionsForManagementQuery request,
            CancellationToken cancellationToken)
        {
            var now = _dateTimeProvider.UtcNow;

            var dataSpec = new SubscriptionsPaginatedSpec(
                request.PageNumber,
                request.PageSize,
                now,
                request.Status,
                request.Search,
                request.Type);

            var countSpec = new SubscriptionsFilteredSpec(
                now,
                request.Status,
                request.Search,
                request.Type);

            var responses = await _unitOfWork.Subscriptions.ListAsync(dataSpec, cancellationToken);
            var totalCount = await _unitOfWork.Subscriptions.CountAsync(countSpec, cancellationToken);

            return new PaginatedResult<GetSubscriptionsForManagementQueryResponse>(
                responses,
                totalCount,
                request.PageNumber,
                request.PageSize);
        }
    }
}
