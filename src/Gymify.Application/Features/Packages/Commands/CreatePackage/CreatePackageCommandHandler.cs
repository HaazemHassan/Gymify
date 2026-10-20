using ErrorOr;
using Gymify.Application.Common.CQRS;
using Gymify.Domain.Entities;
using Gymify.Domain.RepositoriesContracts;

namespace Gymify.Application.Features.Packages.Commands.CreatePackage
{
    public class CreatePackageCommandHandler : ICommandHandler<CreatePackageCommand, CreatePackageCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreatePackageCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ErrorOr<CreatePackageCommandResponse>> Handle(CreatePackageCommand request, CancellationToken cancellationToken)
        {
            var package = new Package(
                request.Name,
                request.Price,
                request.Duration,
                request.Description,
                request.IsActive);

            await _unitOfWork.Packages.AddAsync(package, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new CreatePackageCommandResponse(package.Id);
        }
    }
}
