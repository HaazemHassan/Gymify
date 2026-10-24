using AutoMapper;
using Gymify.Application.Features.Subscriptions.Commands.CreateSubscription.Specifications;
using Gymify.Application.ServicesContracts.Infrastructure;
using Gymify.Domain.Common.Constants;
using Gymify.Domain.Common.Enums;
using Gymify.Domain.Entities;
using Gymify.Domain.RepositoriesContracts;

namespace Gymify.Application.Features.Subscriptions.Commands.CreateSubscription
{
    public class CreateSubscriptionCommandHandler : ICommandHandler<CreateSubscriptionCommand, CreateSubscriptionCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IMapper _mapper;

        public CreateSubscriptionCommandHandler(
            IUnitOfWork unitOfWork,
            IDateTimeProvider dateTimeProvider,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _dateTimeProvider = dateTimeProvider;
            _mapper = mapper;
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

            SubscriptionType subscriptionType;
            int? subscriptionTrainerId;

            switch (package.Type)
            {
                case PackageType.Regular:
                subscriptionType = SubscriptionType.Regular;
                subscriptionTrainerId = null;
                break;

                case PackageType.Private:
                subscriptionType = SubscriptionType.Private;

                if (!package.TrainerId.HasValue)
                {
                    return Error.Conflict(
                        ErrorCodes.Package.PackageNotFound,
                        "Private package does not have an assigned trainer.");
                }

                if (request.TrainerId.HasValue && request.TrainerId.Value != package.TrainerId.Value)
                {
                    return Error.Conflict(
                        ErrorCodes.Subscription.TrainerMismatch,
                        $"This private package is assigned to trainer ID {package.TrainerId.Value}, not {request.TrainerId.Value}.");
                }

                subscriptionTrainerId = package.TrainerId.Value;

                var privateTrainer = await _unitOfWork.Trainers.GetByIdAsync(subscriptionTrainerId.Value, cancellationToken);
                if (privateTrainer is null)
                {
                    return Error.NotFound(
                        ErrorCodes.Trainer.TrainerNotFound,
                        $"Trainer with ID {subscriptionTrainerId.Value} was not found.");
                }

                if (!privateTrainer.IsAvailable)
                {
                    return Error.Conflict(
                        ErrorCodes.Trainer.TrainerUnavailable,
                        $"Trainer with ID {subscriptionTrainerId.Value} is not available.");
                }
                break;

                case PackageType.SharedPrivate:
                subscriptionType = SubscriptionType.Private;

                if (!request.TrainerId.HasValue)
                {
                    return Error.Validation(
                        ErrorCodes.Subscription.TrainerRequired,
                        "Trainer ID is required for shared private packages.",
                        metadata: new Dictionary<string, object> { { "field", nameof(request.TrainerId) } });
                }

                subscriptionTrainerId = request.TrainerId.Value;

                var sharedTrainer = await _unitOfWork.Trainers.GetByIdAsync(subscriptionTrainerId.Value, cancellationToken);
                if (sharedTrainer is null)
                {
                    return Error.NotFound(
                        ErrorCodes.Trainer.TrainerNotFound,
                        $"Trainer with ID {subscriptionTrainerId.Value} was not found.");
                }

                if (!sharedTrainer.IsAvailable)
                {
                    return Error.Conflict(
                        ErrorCodes.Trainer.TrainerUnavailable,
                        $"Trainer with ID {subscriptionTrainerId.Value} is not available.");
                }
                break;

                default:
                throw new ArgumentOutOfRangeException();
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
                subscriptionType,
                subscriptionTrainerId,
                package.Description,
                request.Notes);

            await _unitOfWork.Subscriptions.AddAsync(subscription, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return _mapper.Map<CreateSubscriptionCommandResponse>(subscription);
        }
    }
}
