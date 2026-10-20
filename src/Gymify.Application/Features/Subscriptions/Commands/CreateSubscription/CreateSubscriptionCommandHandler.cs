using ErrorOr;
using Gymify.Application.Common.CQRS;
using Gymify.Application.Features.Subscriptions.Commands.CreateSubscription.Specifications;
using Gymify.Application.ServicesContracts.Infrastructure;
using Gymify.Domain.Common.Constants;
using Gymify.Domain.Entities;
using Gymify.Domain.RepositoriesContracts;

namespace Gymify.Application.Features.Subscriptions.Commands.CreateSubscription
{
    public class CreateSubscriptionCommandHandler : ICommandHandler<CreateSubscriptionCommand, CreateSubscriptionCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDateTimeProvider _dateTimeProvider;

        public CreateSubscriptionCommandHandler(IUnitOfWork unitOfWork, IDateTimeProvider dateTimeProvider)
        {
            _unitOfWork = unitOfWork;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<ErrorOr<CreateSubscriptionCommandResponse>> Handle(CreateSubscriptionCommand request, CancellationToken cancellationToken)
        {
            var customer = await _unitOfWork.Customers.GetByIdAsync(request.CustomerId, cancellationToken);
            if (customer is null)
            {
                return Error.NotFound(
                    ErrorCodes.Customer.CustomerNotFound,
                    $"Customer with ID {request.CustomerId} was not found.");
            }

            var now = _dateTimeProvider.UtcNow;
            var activeSubSpec = new ActiveSubscriptionByCustomerIdSpec(request.CustomerId, now);
            var hasActiveSub = await _unitOfWork.Subscriptions.AnyAsync(activeSubSpec, cancellationToken);
            if (hasActiveSub)
            {
                return Error.Conflict(
                    ErrorCodes.Subscription.CustomerAlreadyHasActiveSubscription,
                    "Customer already has an active subscription.");
            }

            var package = await _unitOfWork.Packages.GetByIdAsync(request.PackageId, cancellationToken);
            if (package is null)
            {
                return Error.NotFound(
                    ErrorCodes.Package.PackageNotFound,
                    $"Package with ID {request.PackageId} was not found.");
            }

            if (!package.IsActive)
            {
                return Error.Conflict(
                    ErrorCodes.Subscription.PackageInactive,
                    $"Package '{package.Name}' is not active.");
            }

            var startDate = now;
            var endDate = now.AddDays(package.Duration);

            var subscription = new Subscription(
                customer.Id,
                package.Id,
                package.Name,
                package.Price,
                package.Duration,
                startDate,
                endDate,
                request.PaymentMethod,
                package.Description,
                request.Notes);

            await _unitOfWork.Subscriptions.AddAsync(subscription, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new CreateSubscriptionCommandResponse
            {
                Id = subscription.Id,
                CustomerId = subscription.CustomerId,
                PackageId = subscription.PackageId,
                PackageName = subscription.PackageName,
                PackagePrice = subscription.PackagePrice,
                PackageDuration = subscription.PackageDuration,
                PackageDescription = subscription.PackageDescription,
                StartDate = subscription.StartDate,
                EndDate = subscription.EndDate,
                PaymentMethod = subscription.PaymentMethod,
                Notes = subscription.Notes
            };
        }
    }
}
