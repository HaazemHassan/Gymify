using ErrorOr;
using Gymify.Application.Common.CQRS;
using Gymify.Application.Features.Lockers.Commands.CreateLocker.Specifications;
using Gymify.Domain.Common.Constants;
using Gymify.Domain.Entities;
using Gymify.Domain.RepositoriesContracts;

namespace Gymify.Application.Features.Lockers.Commands.CreateLocker
{
    public class CreateLockerCommandHandler : ICommandHandler<CreateLockerCommand, CreateLockerCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateLockerCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ErrorOr<CreateLockerCommandResponse>> Handle(CreateLockerCommand request, CancellationToken cancellationToken)
        {
            var spec = new LockerByNumberSpec(request.Number);
            var exists = await _unitOfWork.Lockers.AnyAsync(spec, cancellationToken);
            if (exists)
            {
                return Error.Conflict(
                    ErrorCodes.Locker.NumberAlreadyExists,
                    $"A locker with number '{request.Number}' already exists.");
            }

            var locker = new Locker(request.Number, true);
            await _unitOfWork.Lockers.AddAsync(locker, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new CreateLockerCommandResponse(locker.Id, locker.Number, locker.IsActive);
        }
    }
}
