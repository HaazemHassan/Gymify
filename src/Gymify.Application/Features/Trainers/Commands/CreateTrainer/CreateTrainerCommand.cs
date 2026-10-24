using Gymify.Application.Common.Transaction;
using Gymify.Application.Security;
using Gymify.Application.Security.Markers;
using Gymify.Domain.Common.Enums;

namespace Gymify.Application.Features.Trainers.Commands.CreateTrainer
{
    [Authorize(Permissions = [Permission.TrainersWrite])]
    public class CreateTrainerCommand : ICommand<CreateTrainerCommandResponse>, IAuthorizedRequest, ITransactionalRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;
        public decimal Salary { get; set; }
        public int YearsOfExperience { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsAvailable { get; set; } = true;
    }
}
