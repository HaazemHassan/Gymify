namespace Gymify.Application.Features.CustomerBehaviorNotes.Commands.CreateCustomerBehaviorNote
{
    public class CreateCustomerBehaviorNoteCommandResponse
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int EmployeeId { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateTimeOffset CreatedAt { get; set; }
    }
}
