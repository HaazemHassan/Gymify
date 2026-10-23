namespace Gymify.API.Requests.Kpis.UpdateKpi
{
    public class UpdateKpiRequest
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public bool? IsActive { get; set; }
    }
}
