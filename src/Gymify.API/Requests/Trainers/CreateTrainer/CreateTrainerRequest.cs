namespace Gymify.API.Requests.Trainers.CreateTrainer
{
    public class CreateTrainerRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;
        public decimal Salary { get; set; }
        public int YearsOfExperience { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
