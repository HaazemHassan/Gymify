using ErrorOr;
using Gymify.Domain.Common.Constants;
using Gymify.Domain.Entities.Base;

namespace Gymify.Domain.Entities
{
    public sealed class Attendance : FullAuditableEntity<int>
    {
        private Attendance()
        {
        }

        public Attendance(int customerId, int lockerId, DateTimeOffset checkInAt, string? code = null)
        {
            CustomerId = customerId;
            LockerId = lockerId;
            CheckInAt = checkInAt;
            Code = code;
        }

        public int CustomerId { get; private set; }
        public Customer Customer { get; private set; } = null!;

        public string? Code { get; private set; }

        public int LockerId { get; private set; }
        public Locker Locker { get; private set; } = null!;

        public DateTimeOffset CheckInAt { get; private set; }
        public DateTimeOffset? CheckOutAt { get; private set; }

        public ErrorOr<Success> CheckOut(DateTimeOffset checkOutAt)
        {
            if (CheckOutAt is not null)
            {
                return Error.Conflict(
                    code: ErrorCodes.Attendance.AlreadyCheckedOut,
                    description: "This attendance record has already been checked out.");
            }

            if (checkOutAt < CheckInAt)
            {
                return Error.Validation(
                    code: ErrorCodes.Attendance.InvalidCheckOutTime,
                    description: "Check-out time cannot be earlier than check-in time.");
            }

            CheckOutAt = checkOutAt;
            return Result.Success;
        }
    }
}
