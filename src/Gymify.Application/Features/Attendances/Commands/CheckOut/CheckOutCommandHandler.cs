using ErrorOr;
using Gymify.Application.Common.CQRS;
using Gymify.Application.Features.Attendances.Commands.CheckOut.Specifications;
using Gymify.Application.Features.Attendances.Specifications;
using Gymify.Application.ServicesContracts.Infrastructure;
using Gymify.Domain.Common.Constants;
using Gymify.Domain.RepositoriesContracts;

namespace Gymify.Application.Features.Attendances.Commands.CheckOut
{
    public class CheckOutCommandHandler : ICommandHandler<CheckOutCommand, CheckOutCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDateTimeProvider _dateTimeProvider;

        public CheckOutCommandHandler(IUnitOfWork unitOfWork, IDateTimeProvider dateTimeProvider)
        {
            _unitOfWork = unitOfWork;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<ErrorOr<CheckOutCommandResponse>> Handle(CheckOutCommand request, CancellationToken cancellationToken)
        {
            var customerSpec = new ActiveCustomerByCodeSpec(request.Code);
            var customer = await _unitOfWork.Customers.FirstOrDefaultAsync(customerSpec, cancellationToken);
            if (customer is null)
            {
                return Error.NotFound(
                    ErrorCodes.Customer.CustomerNotFound,
                    $"Customer with code '{request.Code}' was not found.");
            }

            var activeAttendanceSpec = new ActiveAttendanceForCustomerSpec(customer.Id);
            var attendance = await _unitOfWork.Attendances.FirstOrDefaultAsync(activeAttendanceSpec, cancellationToken);
            if (attendance is null)
            {
                return Error.NotFound(
                    ErrorCodes.Attendance.NoActiveCheckInFound,
                    "No active check-in found for this customer.");
            }

            var checkOutTime = _dateTimeProvider.UtcNow;
            var checkOutResult = attendance.CheckOut(checkOutTime);
            if (checkOutResult.IsError)
            {
                return checkOutResult.Errors;
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new CheckOutCommandResponse(
                attendance.Id,
                customer.Id,
                customer.Code,
                attendance.LockerId,
                attendance.CheckInAt,
                checkOutTime);
        }
    }
}
