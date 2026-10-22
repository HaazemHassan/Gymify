using ErrorOr;
using Gymify.Application.Common.CQRS;
using Gymify.Domain.Common.Constants;
using Gymify.Domain.Common.Enums;
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

            var targetType = request.Type ?? package.Type;
            var targetTrainerId = request.TrainerId ?? package.TrainerId;

            if (targetType == PackageType.Private)
            {
                if (!targetTrainerId.HasValue)
                {
                    return Error.Validation(
                        ErrorCodes.Trainer.TrainerNotFound,
                        "Trainer ID is required for private packages.",
                        metadata: new Dictionary<string, object> { { "field", nameof(request.TrainerId) } });
                }

                if (request.TrainerId.HasValue)
                {
                    var trainer = await _unitOfWork.Trainers.GetByIdAsync(request.TrainerId.Value, cancellationToken);
                    if (trainer is null)
                    {
                        return Error.NotFound(
                            ErrorCodes.Trainer.TrainerNotFound,
                            "Trainer was not found.");
                    }

                    if (!trainer.IsActive)
                    {
                        return Error.Validation(
                            ErrorCodes.Trainer.TrainerInactive,
                            "The specified trainer is not active.");
                    }
                }
            }

            package.Update(
                name: request.Name,
                price: request.Price,
                duration: request.Duration,
                description: request.Description,
                isActive: request.IsActive,
                type: request.Type,
                trainerId: request.TrainerId);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new UpdatePackageCommandResponse(
                package.Id,
                package.Name,
                package.Price,
                package.Duration,
                package.Description,
                package.IsActive,
                package.Type,
                package.TrainerId);
        }
    }
}
