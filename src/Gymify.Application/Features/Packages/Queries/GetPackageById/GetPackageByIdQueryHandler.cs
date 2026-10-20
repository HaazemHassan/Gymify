using ErrorOr;
using Gymify.Application.Common.CQRS;
using Gymify.Domain.Common.Constants;
using Gymify.Domain.RepositoriesContracts;

namespace Gymify.Application.Features.Packages.Queries.GetPackageById
{
    public class GetPackageByIdQueryHandler : IQueryHandler<GetPackageByIdQuery, GetPackageByIdQueryResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetPackageByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ErrorOr<GetPackageByIdQueryResponse>> Handle(GetPackageByIdQuery request, CancellationToken cancellationToken)
        {
            // For customers: only active packages can be retrieved
            var package = await _unitOfWork.Packages.GetAsync<GetPackageByIdQueryResponse>(
                p => p.Id == request.Id && p.IsActive,
                cancellationToken);

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
