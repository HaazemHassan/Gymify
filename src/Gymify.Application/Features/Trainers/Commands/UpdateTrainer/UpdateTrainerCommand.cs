using Gymify.Application.Security;
using Gymify.Application.Security.Markers;
using Gymify.Domain.Common.Enums;

namespace Gymify.Application.Features.Trainers.Commands.UpdateTrainer
{
    [Authorize(Permissions = [Permission.TrainersWrite])]
    public class UpdateTrainerCommand : ICommand<UpdateTrainerCommandResponse>, ITrainerResourceRequest
    {
        public int TrainerId { get; set; }
        public string? Name { get; set; }
        public string? PhoneNumber { get; set; }
        public int? YearsOfExperience { get; set; }
        public decimal? Salary { get; set; }
        public bool? IsActive { get; set; }
    }
}
