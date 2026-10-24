namespace Gymify.API.Requests.Trainers.UpdateTrainer
{
    public class UpdateTrainerRequest
    {
        public string? Name { get; set; }
        public string? PhoneNumber { get; set; }
        public int? YearsOfExperience { get; set; }
        public decimal? Salary { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsAvailable { get; set; }
    }
}
