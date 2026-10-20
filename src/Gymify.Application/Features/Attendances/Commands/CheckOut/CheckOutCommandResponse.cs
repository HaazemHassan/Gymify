namespace Gymify.Application.Features.Attendances.Commands.CheckOut
{
    public class CheckOutCommandResponse
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string? CustomerCode { get; set; }
        public int LockerId { get; set; }
        public DateTimeOffset CheckInAt { get; set; }
        public DateTimeOffset CheckOutAt { get; set; }

        public CheckOutCommandResponse()
        {
        }

        public CheckOutCommandResponse(
            int id,
            int customerId,
            string? customerCode,
            int lockerId,
            DateTimeOffset checkInAt,
            DateTimeOffset checkOutAt)
        {
            Id = id;
            CustomerId = customerId;
            CustomerCode = customerCode;
            LockerId = lockerId;
            CheckInAt = checkInAt;
            CheckOutAt = checkOutAt;
        }
    }
}
