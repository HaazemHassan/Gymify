using ErrorOr;
using Gymify.Application.Common.CQRS;
using Gymify.Application.Features.Lockers.Commands.UpdateLocker.Specifications;
using Gymify.Domain.Common.Constants;
using Gymify.Domain.RepositoriesContracts;

namespace Gymify.Application.Features.Lockers.Commands.UpdateLocker
{
    public class UpdateLockerCommandHandler : ICommandHandler<UpdateLockerCommand, UpdateLockerCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateLockerCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ErrorOr<UpdateLockerCommandResponse>> Handle(UpdateLockerCommand request, CancellationToken cancellationToken)
        {
            var locker = await _unitOfWork.Lockers.GetByIdAsync(request.Id, cancellationToken);
            if (locker is null)
            {
                return Error.NotFound(
                    ErrorCodes.Locker.LockerNotFound,
                    $"Locker with id '{request.Id}' was not found.");
            }

            if (request.Number != locker.Number)
            {
                var spec = new LockerByNumberExcludingIdSpec(request.Number, locker.Id);
                var numberExists = await _unitOfWork.Lockers.AnyAsync(spec, cancellationToken);
                if (numberExists)
                {
                    return Error.Conflict(
                        ErrorCodes.Locker.NumberAlreadyExists,
                        $"A locker with number '{request.Number}' already exists.");
                }

                var updateNumberResult = locker.UpdateNumber(request.Number);
                if (updateNumberResult.IsError)
                {
                    return updateNumberResult.Errors;
                }
            }

            if (request.IsActive.HasValue && request.IsActive.Value != locker.IsActive)
            {
                var statusResult = request.IsActive.Value ? locker.Activate() : locker.Deactivate();
                if (statusResult.IsError)
                {
                    return statusResult.Errors;
                }
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new UpdateLockerCommandResponse(locker.Id, locker.Number, locker.IsActive);
        }
    }
}
