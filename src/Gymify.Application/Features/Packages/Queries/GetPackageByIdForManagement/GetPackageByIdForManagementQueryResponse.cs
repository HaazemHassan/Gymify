using Gymify.Domain.Common.Enums;

namespace Gymify.Application.Features.Packages.Queries.GetPackageByIdForManagement
{
    public class GetPackageByIdForManagementQueryResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Duration { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }
        public PackageType Type { get; set; }
        public int? TrainerId { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
    }
}
