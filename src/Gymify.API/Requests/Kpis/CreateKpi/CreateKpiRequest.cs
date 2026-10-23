namespace Gymify.API.Requests.Kpis.CreateKpi
{
    public class CreateKpiRequest
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
