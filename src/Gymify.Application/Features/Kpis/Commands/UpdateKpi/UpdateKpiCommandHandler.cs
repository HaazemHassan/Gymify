using AutoMapper;
using ErrorOr;
using Gymify.Application.Common.CQRS;
using Gymify.Domain.Common.Constants;
using Gymify.Domain.RepositoriesContracts;

namespace Gymify.Application.Features.Kpis.Commands.UpdateKpi
{
    public class UpdateKpiCommandHandler : ICommandHandler<UpdateKpiCommand, UpdateKpiCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateKpiCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ErrorOr<UpdateKpiCommandResponse>> Handle(UpdateKpiCommand request, CancellationToken cancellationToken)
        {
            var kpi = await _unitOfWork.Kpis.GetAsync(k => k.Id == request.Id, cancellationToken);
            if (kpi is null)
            {
                return Error.NotFound(
                    code: ErrorCodes.Kpi.KpiNotFound,
                    description: "KPI was not found.");
            }

            if (!string.IsNullOrWhiteSpace(request.Name) && !string.Equals(kpi.Name, request.Name.Trim(), StringComparison.OrdinalIgnoreCase))
            {
                var nameExists = await _unitOfWork.Kpis.AnyAsync(
                    k => k.Name == request.Name.Trim() && k.Id != request.Id,
                    cancellationToken);

                if (nameExists)
                {
                    return Error.Conflict(
                        code: ErrorCodes.Kpi.KpiNameAlreadyExists,
                        description: "A KPI with this name already exists.");
                }
            }

            kpi.Update(
                name: request.Name?.Trim(),
                description: request.Description?.Trim(),
                isActive: request.IsActive);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return _mapper.Map<UpdateKpiCommandResponse>(kpi);
        }
    }
}
