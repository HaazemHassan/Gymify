using Gymify.Domain.Common.Enums;

namespace Gymify.Application.Features.Customers.Queries.GetCustomerMe
{
    public class GetCustomerMeQueryResponse
    {
        public int Id { get; set; }
        public int DomainUserId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public double? Height { get; set; }
        public double? Weight { get; set; }
        public ApprovalStatus ApprovalStatus { get; set; }
        public bool IsActive { get; set; }
    }
}
