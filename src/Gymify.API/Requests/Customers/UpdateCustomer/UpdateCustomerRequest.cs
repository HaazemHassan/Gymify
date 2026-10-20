using Gymify.Domain.Common.Enums;

namespace Gymify.API.Requests.Customers.UpdateCustomer
{
    public class UpdateCustomerRequest
    {
        public string? Name { get; set; }
        public string? PhoneNumber { get; set; }
        public bool? IsActive { get; set; }

        public string? Code { get; set; }
        public double? Height { get; set; }
        public double? Weight { get; set; }
        public int? Age { get; set; }
        public string? Injuries { get; set; }
        public string? ChronicDiseases { get; set; }
        public double? MusclePercentage { get; set; }
        public double? FatPercentage { get; set; }
        public string? Notes { get; set; }
        public ApprovalStatus? ApprovalStatus { get; set; }
    }
}
