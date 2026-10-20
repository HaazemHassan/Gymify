using ErrorOr;
using Gymify.Domain.Common.Constants;
using Gymify.Domain.Entities.Base;

namespace Gymify.Domain.Entities
{
    public sealed class Trainer : FullAuditableEntity<int>
    {
        private Trainer()
        {
        }

        public Trainer(int employeeId, int yearsOfExperience = 0)
        {
            EmployeeId = employeeId;
            YearsOfExperience = yearsOfExperience;
        }

        public int EmployeeId { get; private set; }
        public Employee Employee { get; private set; } = null!;
        public int YearsOfExperience { get; private set; }

        public ErrorOr<Success> UpdateExperience(int yearsOfExperience)
        {
            if (yearsOfExperience < 0)
            {
                return Error.Validation(
                    code: ErrorCodes.Trainer.InvalidExperience,
                    description: "Years of experience cannot be negative.");
            }

            YearsOfExperience = yearsOfExperience;
            return Result.Success;
        }
    }
}
