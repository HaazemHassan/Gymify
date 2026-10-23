using AutoMapper;
using Gymify.Application.Features.Kpis.Commands.UpdateKpi;

namespace Gymify.API.Requests.Kpis.UpdateKpi
{
    public class UpdateKpiRequestMappingProfile : Profile
    {
        public UpdateKpiRequestMappingProfile()
        {
            CreateMap<UpdateKpiRequest, UpdateKpiCommand>();
        }
    }
}
