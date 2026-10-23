using ErrorOr;
using Gymify.Application.Common.CQRS;
using Gymify.Application.Contracts.Api;
using Gymify.Domain.Common.Constants;
using Gymify.Domain.Entities;
using Gymify.Domain.RepositoriesContracts;

namespace Gymify.Application.Features.CustomerBehaviorNotes.Commands.CreateCustomerBehaviorNote
{
    public class CreateCustomerBehaviorNoteCommandHandler : ICommandHandler<CreateCustomerBehaviorNoteCommand, CreateCustomerBehaviorNoteCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public CreateCustomerBehaviorNoteCommandHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task<ErrorOr<CreateCustomerBehaviorNoteCommandResponse>> Handle(CreateCustomerBehaviorNoteCommand request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId;
            if (!userId.HasValue)
            {
                return Error.Unauthorized(
                    code: ErrorCodes.Authorization.NotAuthenticated,
                    description: "User is not authenticated.");
            }

            var customer = await _unitOfWork.Customers.GetByIdAsync(request.CustomerId, cancellationToken);
            if (customer is null)
            {
                return Error.NotFound(
                    code: ErrorCodes.Customer.CustomerNotFound,
                    description: $"Customer with ID {request.CustomerId} was not found.");
            }

            var employee = await _unitOfWork.Employees.GetAsync(e => e.DomainUserId == userId.Value, cancellationToken);
            if (employee is null)
            {
                return Error.Forbidden(
                    code: ErrorCodes.Authorization.NotAllowed,
                    description: "Only employees can add customer behavior notes.");
            }

            var note = new CustomerBehaviorNote(request.CustomerId, employee.Id, request.Description.Trim());

            await _unitOfWork.CustomerBehaviorNotes.AddAsync(note, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new CreateCustomerBehaviorNoteCommandResponse
            {
                Id = note.Id,
                CustomerId = note.CustomerId,
                EmployeeId = note.EmployeeId,
                Description = note.Description,
                CreatedAt = note.CreatedAt
            };
        }
    }
}
