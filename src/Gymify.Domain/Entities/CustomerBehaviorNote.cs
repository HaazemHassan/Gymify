using Gymify.Domain.Entities.Base;

namespace Gymify.Domain.Entities
{
    public sealed class CustomerBehaviorNote : FullAuditableEntity<int>
    {
        private CustomerBehaviorNote()
        {
        }

        public CustomerBehaviorNote(int customerId, int employeeId, string description)
        {
            CustomerId = customerId;
            EmployeeId = employeeId;
            Description = description;
        }

        public int CustomerId { get; private set; }
        public Customer Customer { get; private set; } = null!;

        public int EmployeeId { get; private set; }
        public Employee Employee { get; private set; } = null!;

        public string Description { get; private set; } = string.Empty;
    }
}
