using AutoMapper;
using Gymify.Domain.Entities;

namespace Gymify.Application.Features.Kpis.Commands.UpdateKpi
{
    public class UpdateKpiCommandMappingProfile : Profile
    {
        public UpdateKpiCommandMappingProfile()
        {
            CreateMap<Kpi, UpdateKpiCommandResponse>();
        }
    }
}
