using ErrorOr;
using Gymify.Application.Common.CQRS;
using Gymify.Domain.Common.Constants;
using Gymify.Domain.RepositoriesContracts;

namespace Gymify.Application.Features.Packages.Commands.UpdatePackage
{
    public class UpdatePackageCommandHandler : ICommandHandler<UpdatePackageCommand, UpdatePackageCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdatePackageCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ErrorOr<UpdatePackageCommandResponse>> Handle(UpdatePackageCommand request, CancellationToken cancellationToken)
        {
            var package = await _unitOfWork.Packages.GetByIdAsync(request.Id, cancellationToken);
            if (package is null)
            {
                return Error.NotFound(
                    code: ErrorCodes.Package.PackageNotFound,
                    description: "Package was not found.");
            }

            package.Update(
                name: request.Name,
                price: request.Price,
                duration: request.Duration,
                description: request.Description,
                isActive: request.IsActive);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new UpdatePackageCommandResponse(
                package.Id,
                package.Name,
                package.Price,
                package.Duration,
                package.Description,
                package.IsActive);
        }
    }
}
