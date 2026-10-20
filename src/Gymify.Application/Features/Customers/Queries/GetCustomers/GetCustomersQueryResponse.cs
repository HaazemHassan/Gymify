using Gymify.Domain.Common.Enums;

namespace Gymify.Application.Features.Customers.Queries.GetCustomers
{
    public class GetCustomersQueryResponse
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public string? Code { get; set; }
        public double? Height { get; set; }
        public double? Weight { get; set; }
        public int? Age { get; set; }
        public ApprovalStatus ApprovalStatus { get; set; }
        public bool IsActive { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
    }
}
