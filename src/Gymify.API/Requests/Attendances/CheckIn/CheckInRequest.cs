namespace Gymify.API.Requests.Attendances.CheckIn
{
    public class CheckInRequest
    {
        public string Code { get; set; } = string.Empty;
        public int LockerId { get; set; }
    }
}
