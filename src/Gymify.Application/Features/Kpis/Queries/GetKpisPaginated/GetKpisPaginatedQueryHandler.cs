using ErrorOr;
using Gymify.Application.Common.CQRS;
using Gymify.Application.Common.Pagination;
using Gymify.Application.Features.Kpis.Queries.GetKpisPaginated.Specifications;
using Gymify.Application.Features.Kpis.Specifications;
using Gymify.Domain.RepositoriesContracts;

namespace Gymify.Application.Features.Kpis.Queries.GetKpisPaginated
{
    public class GetKpisPaginatedQueryHandler : IQueryHandler<GetKpisPaginatedQuery, PaginatedResult<GetKpisPaginatedQueryResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetKpisPaginatedQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ErrorOr<PaginatedResult<GetKpisPaginatedQueryResponse>>> Handle(GetKpisPaginatedQuery request, CancellationToken cancellationToken)
        {
            var dataSpec = new KpisPaginatedSpec(request.PageNumber, request.PageSize, request.Search, request.IsActive);
            var countSpec = new KpisFilteredSpec(request.Search, request.IsActive);

            var items = await _unitOfWork.Kpis.ListAsync<GetKpisPaginatedQueryResponse>(dataSpec, cancellationToken);
            var totalCount = await _unitOfWork.Kpis.CountAsync(countSpec, cancellationToken);

            return new PaginatedResult<GetKpisPaginatedQueryResponse>(items, totalCount, request.PageNumber, request.PageSize);
        }
    }
}
