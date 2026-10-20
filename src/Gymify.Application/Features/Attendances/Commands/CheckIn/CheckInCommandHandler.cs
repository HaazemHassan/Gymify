using Gymify.Application.Features.Attendances.Commands.CheckIn.Specifications;
using Gymify.Application.Features.Attendances.Specifications;
using Gymify.Application.ServicesContracts.Infrastructure;
using Gymify.Domain.Common.Constants;
using Gymify.Domain.Entities;
using Gymify.Domain.RepositoriesContracts;

namespace Gymify.Application.Features.Attendances.Commands.CheckIn
{
    public class CheckInCommandHandler : ICommandHandler<CheckInCommand, CheckInCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDateTimeProvider _dateTimeProvider;

        public CheckInCommandHandler(IUnitOfWork unitOfWork, IDateTimeProvider dateTimeProvider)
        {
            _unitOfWork = unitOfWork;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<ErrorOr<CheckInCommandResponse>> Handle(CheckInCommand request, CancellationToken cancellationToken)
        {
            var customerSpec = new CustomerByCodeSpec(request.Code);
            var customer = await _unitOfWork.Customers.FirstOrDefaultAsync(customerSpec, cancellationToken);
            if (customer is null)
            {
                return Error.NotFound(
                    ErrorCodes.Customer.CustomerNotFound,
                    $"Customer with code '{request.Code}' was not found.");
            }

            var now = _dateTimeProvider.UtcNow;

            var latestSubscription = await _unitOfWork.Subscriptions.FirstOrDefaultAsync(
                new LatestSubscriptionByCustomerIdSpec(customer.Id), cancellationToken);

            var isExpired = latestSubscription == null || latestSubscription.EndDate < now;
            var hasActiveSubscription = latestSubscription != null && latestSubscription.IsActive(now);

            if (!hasActiveSubscription)
            {
                return Error.Conflict(
                    ErrorCodes.Attendance.CustomerHasNoActiveSubscription,
                    isExpired && latestSubscription != null
                        ? $"Customer subscription expired on {latestSubscription.EndDate:yyyy-MM-dd}."
                        : "Customer does not have an active subscription.");
            }

            // 2. Active attendance check
            var activeCustomerAttendanceSpec = new ActiveAttendanceByCustomerIdSpec(customer.Id);
            var hasActiveAttendance = await _unitOfWork.Attendances.AnyAsync(activeCustomerAttendanceSpec, cancellationToken);
            if (hasActiveAttendance)
            {
                return Error.Conflict(
                    ErrorCodes.Attendance.CustomerAlreadyCheckedIn,
                    "Customer already has an active check-in.");
            }

            // 3. Locker verification
            var locker = await _unitOfWork.Lockers.GetByIdAsync(request.LockerId, cancellationToken);
            if (locker is null)
            {
                return Error.NotFound(
                    ErrorCodes.Locker.LockerNotFound,
                    $"Locker with id '{request.LockerId}' was not found.");
            }

            if (!locker.IsActive)
            {
                return Error.Validation(
                    ErrorCodes.Locker.LockerInactive,
                    $"Locker '{locker.Number}' is not active.");
            }

            var activeLockerAttendanceSpec = new ActiveAttendanceByLockerIdSpec(locker.Id);
            var isLockerOccupied = await _unitOfWork.Attendances.AnyAsync(activeLockerAttendanceSpec, cancellationToken);
            if (isLockerOccupied)
            {
                return Error.Conflict(
                    ErrorCodes.Attendance.LockerNotAvailable,
                    $"Locker '{locker.Number}' is already occupied.");
            }

            // 4. Query previous last check-in date
            var lastAttendance = await _unitOfWork.Attendances.FirstOrDefaultAsync(
                new LastCheckInByCustomerIdSpec(customer.Id), cancellationToken);
            var lastCheckIn = lastAttendance?.CheckInAt;

            // 5. Record new attendance
            var attendance = new Attendance(customer.Id, locker.Id, now, customer.Code);
            await _unitOfWork.Attendances.AddAsync(attendance, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new CheckInCommandResponse
            {
                Id = attendance.Id,
                CustomerId = customer.Id,
                CustomerCode = customer.Code,
                FullName = customer.DomainUser?.Name ?? string.Empty,
                Email = customer.DomainUser?.Email ?? string.Empty,
                PhoneNumber = customer.DomainUser?.PhoneNumber,
                SubscriptionEndDate = latestSubscription?.EndDate,
                LastCheckInAt = lastCheckIn,
                LockerId = locker.Id,
                LockerNumber = locker.Number,
                CheckInAt = attendance.CheckInAt
            };
        }
    }
}
