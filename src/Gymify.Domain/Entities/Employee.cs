using Gymify.Domain.Entities.Base;

namespace Gymify.Domain.Entities
{
    public sealed class Employee : FullAuditableEntity<int>
    {
        private Employee()
        {
        }

        public Employee(
            int domainUserId,
            decimal salary,
            bool isActive = true)
        {
            DomainUserId = domainUserId;
            Salary = salary;
            IsActive = isActive;
        }

        public int DomainUserId { get; private set; }
        public DomainUser DomainUser { get; private set; } = null!;

        public decimal Salary { get; private set; }
        public bool IsActive { get; private set; } = true;

        public Trainer? Trainer { get; private set; }

        public void UpdateSalary(decimal salary)
        {
            Salary = salary;
        }

        public void SetActive(bool isActive)
        {
            IsActive = isActive;
        }
    }
}
