using Gymify.Domain.Common.Enums;

namespace Gymify.Application.Features.Trainers.Queries.GetTrainerCustomers
{
    public class GetTrainerCustomersQueryResponse
    {
        public int Id { get; set; }
        public int SubscriptionId { get; set; }
        public int CustomerId { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public string? Code { get; set; }
        public ApprovalStatus ApprovalStatus { get; set; }
        public bool IsActive { get; set; }
        public string PackageName { get; set; } = string.Empty;
        public decimal PackagePrice { get; set; }
        public int PackageDuration { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset EndDate { get; set; }
    }
}
