namespace Gymify.Application.Features.Attendances.Commands.CheckIn
{
    public class CheckInCommandResponse
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string? CustomerCode { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }

        public DateTimeOffset? SubscriptionEndDate { get; set; }
        public DateTimeOffset? LastCheckInAt { get; set; }

        public int LockerId { get; set; }
        public int LockerNumber { get; set; }
        public DateTimeOffset CheckInAt { get; set; }

        public CheckInCommandResponse()
        {
        }
    }
}
