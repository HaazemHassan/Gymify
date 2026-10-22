using ErrorOr;
using Gymify.Application.Common.CQRS;
using Gymify.Application.Common.Pagination;
using Gymify.Application.Features.Packages.Queries.GetPackagesForCustomers.Specifications;
using Gymify.Application.Features.Packages.Specifications;
using Gymify.Domain.RepositoriesContracts;

namespace Gymify.Application.Features.Packages.Queries.GetPackagesForCustomers
{
    public class GetPackagesForCustomersQueryHandler : IQueryHandler<GetPackagesForCustomersQuery, PaginatedResult<GetPackagesForCustomersQueryResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetPackagesForCustomersQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ErrorOr<PaginatedResult<GetPackagesForCustomersQueryResponse>>> Handle(GetPackagesForCustomersQuery request, CancellationToken cancellationToken)
        {
            var dataSpec = new ActivePackagesPaginatedSpec(request.PageNumber, request.PageSize, request.Search, request.Types);
            var countSpec = new PackagesFilteredSpec(request.Search, isActive: true, types: request.Types);

            var packages = await _unitOfWork.Packages.ListAsync<GetPackagesForCustomersQueryResponse>(dataSpec, cancellationToken);
            var totalCount = await _unitOfWork.Packages.CountAsync(countSpec, cancellationToken);

            return new PaginatedResult<GetPackagesForCustomersQueryResponse>(packages, totalCount, request.PageNumber, request.PageSize);
        }
    }
}
