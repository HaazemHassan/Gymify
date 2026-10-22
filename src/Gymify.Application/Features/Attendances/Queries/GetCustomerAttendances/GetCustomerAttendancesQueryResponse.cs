namespace Gymify.Application.Features.Attendances.Queries.GetCustomerAttendances
{
    public class GetCustomerAttendancesQueryResponse
    {
        public int CustomerId { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public int TotalAttendances { get; set; }
        public List<CustomerAttendanceItemResponse> Attendances { get; set; } = [];
    }
}
