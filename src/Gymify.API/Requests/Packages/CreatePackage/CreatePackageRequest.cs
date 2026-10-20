namespace Gymify.Api.Requests.Packages.CreatePackage
{
    public class CreatePackageRequest
    {
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Duration { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
