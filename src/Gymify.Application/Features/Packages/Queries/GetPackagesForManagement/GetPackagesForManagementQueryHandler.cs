using ErrorOr;
using Gymify.Application.Common.CQRS;
using Gymify.Application.Common.Pagination;
using Gymify.Application.Features.Packages.Queries.GetPackagesForManagement.Specifications;
using Gymify.Application.Features.Packages.Specifications;
using Gymify.Domain.RepositoriesContracts;

namespace Gymify.Application.Features.Packages.Queries.GetPackagesForManagement
{
    public class GetPackagesForManagementQueryHandler : IQueryHandler<GetPackagesForManagementQuery, PaginatedResult<GetPackagesForManagementQueryResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetPackagesForManagementQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ErrorOr<PaginatedResult<GetPackagesForManagementQueryResponse>>> Handle(GetPackagesForManagementQuery request, CancellationToken cancellationToken)
        {
            var dataSpec = new ManagementPackagesPaginatedSpec(request.PageNumber, request.PageSize, request.Search, request.Types, request.TrainerId, request.IsActive);
            var countSpec = new PackagesFilteredSpec(request.Search, isActive: request.IsActive, types: request.Types, trainerId: request.TrainerId);

            var packages = await _unitOfWork.Packages.ListAsync<GetPackagesForManagementQueryResponse>(dataSpec, cancellationToken);
            var totalCount = await _unitOfWork.Packages.CountAsync(countSpec, cancellationToken);

            return new PaginatedResult<GetPackagesForManagementQueryResponse>(packages, totalCount, request.PageNumber, request.PageSize);
        }
    }
}
