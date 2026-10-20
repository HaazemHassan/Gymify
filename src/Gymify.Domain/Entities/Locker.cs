using ErrorOr;
using Gymify.Domain.Common.Constants;
using Gymify.Domain.Entities.Base;

namespace Gymify.Domain.Entities
{
    public sealed class Locker : FullAuditableEntity<int>
    {
        private Locker()
        {
        }

        public Locker(int number, bool isActive = true)
        {
            Number = number;
            IsActive = isActive;
        }

        public int Number { get; private set; }
        public bool IsActive { get; private set; } = true;

        public ICollection<Attendance> Attendances { get; private set; } = new List<Attendance>();

        public ErrorOr<Success> Activate()
        {
            if (IsActive)
            {
                return Error.Conflict(
                    code: ErrorCodes.Locker.AlreadyActive,
                    description: "Locker is already active.");
            }

            IsActive = true;
            return Result.Success;
        }

        public ErrorOr<Success> Deactivate()
        {
            if (!IsActive)
            {
                return Error.Conflict(
                    code: ErrorCodes.Locker.AlreadyInactive,
                    description: "Locker is already inactive.");
            }

            IsActive = false;
            return Result.Success;
        }

        public ErrorOr<Success> UpdateNumber(int number)
        {
            if (number <= 0)
            {
                return Error.Validation(
                    code: ErrorCodes.Locker.InvalidNumber,
                    description: "Locker number must be greater than zero.");
            }

            Number = number;
            return Result.Success;
        }
    }
}
