using ErrorOr;
using Gymify.Application.Common.CQRS;
using Gymify.Domain.Common.Constants;
using Gymify.Domain.RepositoriesContracts;

namespace Gymify.Application.Features.Lockers.Commands.ChangeLockerStatus
{
    public class ChangeLockerStatusCommandHandler : ICommandHandler<ChangeLockerStatusCommand, ChangeLockerStatusCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public ChangeLockerStatusCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ErrorOr<ChangeLockerStatusCommandResponse>> Handle(ChangeLockerStatusCommand request, CancellationToken cancellationToken)
        {
            var locker = await _unitOfWork.Lockers.GetByIdAsync(request.Id, cancellationToken);
            if (locker is null)
            {
                return Error.NotFound(
                    ErrorCodes.Locker.LockerNotFound,
                    $"Locker with id '{request.Id}' was not found.");
            }

            var statusResult = request.IsActive ? locker.Activate() : locker.Deactivate();
            if (statusResult.IsError)
            {
                return statusResult.Errors;
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new ChangeLockerStatusCommandResponse(locker.Id, locker.Number, locker.IsActive);
        }
    }
}
