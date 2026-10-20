using AutoMapper;
using ErrorOr;
using Gymify.Application.Common.CQRS;
using Gymify.Domain.Common.Constants;
using Gymify.Domain.RepositoriesContracts;

namespace Gymify.Application.Features.Customers.Commands.ChangeCustomerApprovalStatus
{
    public class ChangeCustomerApprovalStatusCommandHandler : ICommandHandler<ChangeCustomerApprovalStatusCommand, ChangeCustomerApprovalStatusCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ChangeCustomerApprovalStatusCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ErrorOr<ChangeCustomerApprovalStatusCommandResponse>> Handle(ChangeCustomerApprovalStatusCommand request, CancellationToken cancellationToken)
        {
            var customer = await _unitOfWork.Customers.GetByIdAsync(request.CustomerId, cancellationToken);
            if (customer is null)
            {
                return Error.NotFound(
                    code: ErrorCodes.Customer.CustomerNotFound,
                    description: $"Customer with ID {request.CustomerId} was not found.");
            }

            var statusResult = customer.UpdateApprovalStatus(request.Status);
            if (statusResult.IsError)
            {
                return statusResult.Errors;
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return _mapper.Map<ChangeCustomerApprovalStatusCommandResponse>(customer);
        }
    }
}
