namespace Gymify.Api.Requests.Packages.UpdatePackage
{
    public class UpdatePackageRequest
    {
        public string? Name { get; set; }
        public decimal? Price { get; set; }
        public int? Duration { get; set; }
        public string? Description { get; set; }
        public bool? IsActive { get; set; }
    }
}
