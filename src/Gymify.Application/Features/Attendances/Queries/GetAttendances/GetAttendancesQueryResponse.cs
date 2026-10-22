namespace Gymify.Application.Features.Attendances.Queries.GetAttendances
{
    public class GetAttendancesQueryResponse
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; } = null!;
        public string? CustomerEmail { get; set; }
        public string? CustomerPhoneNumber { get; set; }
        public string? AttendanceCode { get; set; }
        public int LockerId { get; set; }
        public int LockerNumber { get; set; }
        public DateTimeOffset CheckInAt { get; set; }
        public DateTimeOffset? CheckOutAt { get; set; }
        public bool IsCurrentlyAttending { get; set; }
    }
}
