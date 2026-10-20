namespace Gymify.Application.Features.Trainers.Queries.GetTrainersForCustomers
{
    public class GetTrainersForCustomersQueryResponse
    {
        public int Id { get; set; }
        public int TrainerId { get; set; }
        public int EmployeeId { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public int YearsOfExperience { get; set; }

        public GetTrainersForCustomersQueryResponse()
        {
        }

        public GetTrainersForCustomersQueryResponse(
            int id,
            int trainerId,
            int employeeId,
            int userId,
            string name,
            string email,
            string? phoneNumber,
            int yearsOfExperience)
        {
            Id = id;
            TrainerId = trainerId;
            EmployeeId = employeeId;
            UserId = userId;
            Name = name;
            Email = email;
            PhoneNumber = phoneNumber;
            YearsOfExperience = yearsOfExperience;
        }
    }
}
