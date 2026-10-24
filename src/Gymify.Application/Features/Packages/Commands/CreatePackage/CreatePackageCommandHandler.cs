using ErrorOr;
using Gymify.Application.Common.CQRS;
using Gymify.Domain.Common.Constants;
using Gymify.Domain.Common.Enums;
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
            if (request.Type == PackageType.Private && request.TrainerId.HasValue)
            {
                var trainer = await _unitOfWork.Trainers.GetByIdAsync(request.TrainerId.Value, cancellationToken);
                if (trainer is null)
                {
                    return Error.NotFound(
                        ErrorCodes.Trainer.TrainerNotFound,
                        "Trainer was not found.");
                }

                if (!trainer.IsAvailable)
                {
                    return Error.Validation(
                        ErrorCodes.Trainer.TrainerUnavailable,
                        "The specified trainer is not available.");
                }
            }

            var package = new Package(
                request.Name,
                request.Price,
                request.Duration,
                request.Description,
                request.IsActive,
                request.Type,
                request.TrainerId);

            await _unitOfWork.Packages.AddAsync(package, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new CreatePackageCommandResponse(package.Id);
        }
    }
}
