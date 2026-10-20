using ErrorOr;
using Gymify.Application.Common.CQRS;
using Gymify.Domain.Common.Constants;
using Gymify.Domain.RepositoriesContracts;

namespace Gymify.Application.Features.Packages.Queries.GetPackageByIdForManagement
{
    public class GetPackageByIdForManagementQueryHandler : IQueryHandler<GetPackageByIdForManagementQuery, GetPackageByIdForManagementQueryResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetPackageByIdForManagementQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ErrorOr<GetPackageByIdForManagementQueryResponse>> Handle(GetPackageByIdForManagementQuery request, CancellationToken cancellationToken)
        {
            // For management: can retrieve package regardless of whether it's active or inactive
            var package = await _unitOfWork.Packages.GetByIdAsync<GetPackageByIdForManagementQueryResponse>(request.Id, cancellationToken);
            if (package is null)
            {
                return Error.NotFound(
                    code: ErrorCodes.Package.PackageNotFound,
                    description: "Package was not found.");
            }

            return package;
        }
    }
}
