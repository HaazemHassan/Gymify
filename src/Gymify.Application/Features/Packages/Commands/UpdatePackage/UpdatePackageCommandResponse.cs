using Gymify.Domain.Common.Enums;

namespace Gymify.Application.Features.Packages.Commands.UpdatePackage
{
    public class UpdatePackageCommandResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Duration { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }
        public PackageType Type { get; set; }
        public int? TrainerId { get; set; }

        public UpdatePackageCommandResponse()
        {
        }

        public UpdatePackageCommandResponse(
            int id,
            string name,
            decimal price,
            int duration,
            string? description,
            bool isActive,
            PackageType type,
            int? trainerId)
        {
            Id = id;
            Name = name;
            Price = price;
            Duration = duration;
            Description = description;
            IsActive = isActive;
            Type = type;
            TrainerId = trainerId;
        }
    }
}
