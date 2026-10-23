using AutoMapper;
using Gymify.Application.Features.Kpis.Commands.CreateKpi;

namespace Gymify.API.Requests.Kpis.CreateKpi
{
    public class CreateKpiRequestMappingProfile : Profile
    {
        public CreateKpiRequestMappingProfile()
        {
            CreateMap<CreateKpiRequest, CreateKpiCommand>();
        }
    }
}
