namespace Gymify.API.Requests.Customers.CreateCustomer
{
    public class CreateCustomerRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;

        public string? Code { get; set; }
        public double? Height { get; set; }
        public double? Weight { get; set; }
        public int? Age { get; set; }
        public string? Injuries { get; set; }
        public string? ChronicDiseases { get; set; }
        public double? MusclePercentage { get; set; }
        public double? FatPercentage { get; set; }
        public string? Notes { get; set; }
    }
}
