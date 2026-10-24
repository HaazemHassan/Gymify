using ErrorOr;
using Gymify.Application.Common.CQRS;
using Gymify.Application.Common.Pagination;
using Gymify.Application.Features.Subscriptions.Queries.GetCustomerSubscriptions.Specifications;
using Gymify.Application.Features.Subscriptions.Specifications;
using Gymify.Application.ServicesContracts.Infrastructure;
using Gymify.Domain.Common.Constants;
using Gymify.Domain.RepositoriesContracts;

namespace Gymify.Application.Features.Subscriptions.Queries.GetCustomerSubscriptions
{
    public class GetCustomerSubscriptionsQueryHandler
        : IQueryHandler<GetCustomerSubscriptionsQuery, PaginatedResult<GetCustomerSubscriptionsQueryResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDateTimeProvider _dateTimeProvider;

        public GetCustomerSubscriptionsQueryHandler(
            IUnitOfWork unitOfWork,
            IDateTimeProvider dateTimeProvider)
        {
            _unitOfWork = unitOfWork;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<ErrorOr<PaginatedResult<GetCustomerSubscriptionsQueryResponse>>> Handle(
            GetCustomerSubscriptionsQuery request,
            CancellationToken cancellationToken)
        {
            var customerExists = await _unitOfWork.Customers.AnyAsync(
                c => c.Id == request.CustomerId,
                cancellationToken);

            if (!customerExists)
            {
                return Error.NotFound(
                    code: ErrorCodes.Customer.CustomerNotFound,
                    description: $"Customer with ID {request.CustomerId} was not found.");
            }

            var now = _dateTimeProvider.UtcNow;

            var dataSpec = new CustomerSubscriptionsPaginatedSpec(
                request.CustomerId,
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
                request.Type,
                customerId: request.CustomerId);

            var responses = await _unitOfWork.Subscriptions.ListAsync(dataSpec, cancellationToken);
            var totalCount = await _unitOfWork.Subscriptions.CountAsync(countSpec, cancellationToken);

            return new PaginatedResult<GetCustomerSubscriptionsQueryResponse>(
                responses,
                totalCount,
                request.PageNumber,
                request.PageSize);
        }
    }
}
