namespace Gymify.Application.Features.Trainers.Commands.CreateTrainer
{
    public class CreateTrainerCommandResponse
    {
        public int TrainerId { get; set; }
        public int EmployeeId { get; set; }
        public int UserId { get; set; }

        public CreateTrainerCommandResponse()
        {
        }

        public CreateTrainerCommandResponse(int trainerId, int employeeId, int userId)
        {
            TrainerId = trainerId;
            EmployeeId = employeeId;
            UserId = userId;
        }
    }
}
