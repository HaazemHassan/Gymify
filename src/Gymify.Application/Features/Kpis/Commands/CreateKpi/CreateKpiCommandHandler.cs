using ErrorOr;
using Gymify.Application.Common.CQRS;
using Gymify.Domain.Common.Constants;
using Gymify.Domain.Entities;
using Gymify.Domain.RepositoriesContracts;

namespace Gymify.Application.Features.Kpis.Commands.CreateKpi
{
    public class CreateKpiCommandHandler : ICommandHandler<CreateKpiCommand, CreateKpiCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateKpiCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ErrorOr<CreateKpiCommandResponse>> Handle(CreateKpiCommand request, CancellationToken cancellationToken)
        {
            var nameExists = await _unitOfWork.Kpis.AnyAsync(k => k.Name == request.Name, cancellationToken);
            if (nameExists)
            {
                return Error.Conflict(
                    code: ErrorCodes.Kpi.KpiNameAlreadyExists,
                    description: "A KPI with this name already exists.");
            }

            var kpi = new Kpi(request.Name, request.Description, request.IsActive);

            await _unitOfWork.Kpis.AddAsync(kpi, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new CreateKpiCommandResponse(kpi.Id);
        }
    }
}
