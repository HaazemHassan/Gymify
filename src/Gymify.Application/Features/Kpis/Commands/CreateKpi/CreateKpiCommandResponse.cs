namespace Gymify.Application.Features.Kpis.Commands.CreateKpi
{
    public class CreateKpiCommandResponse
    {
        public int Id { get; set; }

        public CreateKpiCommandResponse()
        {
        }

        public CreateKpiCommandResponse(int id)
        {
            Id = id;
        }
    }
}
