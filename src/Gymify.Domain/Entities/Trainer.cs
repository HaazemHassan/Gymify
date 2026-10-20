using Gymify.Domain.Entities.Base;

namespace Gymify.Domain.Entities
{
    public sealed class Trainer : FullAuditableEntity<int>
    {
        private Trainer()
        {
        }

        public Trainer(int employeeId)
        {
            EmployeeId = employeeId;
        }

        public int EmployeeId { get; private set; }
        public Employee Employee { get; private set; } = null!;
    }
}
