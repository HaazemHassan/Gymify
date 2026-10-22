namespace Gymify.Application.Features.Attendances.Queries.GetCustomerAttendances
{
    public class CustomerAttendanceItemResponse
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public DateTimeOffset CheckInAt { get; set; }
        public DateTimeOffset? CheckOutAt { get; set; }
        public int LockerId { get; set; }
        public int LockerNumber { get; set; }
        public string? AttendanceCode { get; set; }
        public TimeSpan? Duration { get; set; }
        public bool IsCurrentlyAttending { get; set; }
    }
}
