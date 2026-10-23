namespace Gymify.Application.Features.Trainers.Queries.GetTrainerMe
{
    public class GetTrainerMeQueryResponse
    {
        public int Id { get; set; }
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
