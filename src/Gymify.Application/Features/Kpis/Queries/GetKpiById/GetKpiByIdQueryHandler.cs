using ErrorOr;
using Gymify.Application.Common.CQRS;
using Gymify.Domain.Common.Constants;
using Gymify.Domain.RepositoriesContracts;

namespace Gymify.Application.Features.Kpis.Queries.GetKpiById
{
    public class GetKpiByIdQueryHandler : IQueryHandler<GetKpiByIdQuery, GetKpiByIdQueryResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetKpiByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ErrorOr<GetKpiByIdQueryResponse>> Handle(GetKpiByIdQuery request, CancellationToken cancellationToken)
        {
            var kpi = await _unitOfWork.Kpis.GetByIdAsync<GetKpiByIdQueryResponse>(request.Id, cancellationToken);
            if (kpi is null)
            {
                return Error.NotFound(
                    code: ErrorCodes.Kpi.KpiNotFound,
                    description: "KPI was not found.");
            }

            return kpi;
        }
    }
}
