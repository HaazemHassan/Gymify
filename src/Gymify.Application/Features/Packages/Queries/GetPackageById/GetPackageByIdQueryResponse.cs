namespace Gymify.Application.Features.Packages.Queries.GetPackageById
{
    public class GetPackageByIdQueryResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Duration { get; set; }
        public string? Description { get; set; }
    }
}
