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

        public Trainer(int employeeId, int yearsOfExperience = 0, bool isActive = true)
        {
            EmployeeId = employeeId;
            YearsOfExperience = yearsOfExperience;
            IsActive = isActive;
        }

        public Trainer(Employee employee, int yearsOfExperience = 0, bool isActive = true)
        {
            Employee = employee;
            YearsOfExperience = yearsOfExperience;
            IsActive = isActive;
        }

        public int EmployeeId { get; private set; }
        public Employee Employee { get; private set; } = null!;
        public int YearsOfExperience { get; private set; }
        public bool IsActive { get; private set; } = true;

        public void SetActive(bool isActive)
        {
            IsActive = isActive;
        }

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
