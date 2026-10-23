namespace Gymify.Application.Features.CustomerBehaviorNotes.Queries.GetCustomerBehaviorNotes
{
    public class GetCustomerBehaviorNotesQueryResponse
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int EmployeeId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string EmployeeName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTimeOffset CreatedAt { get; set; }
    }
}
