namespace Gymify.Application.Features.Trainers.Commands.UpdateTrainer
{
    public class UpdateTrainerCommandResponse
    {
        public int Id { get; set; }
        public int TrainerId { get; set; }
        public int EmployeeId { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public int YearsOfExperience { get; set; }
        public decimal Salary { get; set; }
        public bool IsTrainerActive { get; set; }
    }
}
