using AutoMapper;
using ErrorOr;
using Gymify.Application.Common.CQRS;
using Gymify.Application.Features.Customers.Commands.UpdateCustomer.Specifications;
using Gymify.Application.ServicesContracts.Infrastructure;
using Gymify.Domain.Common.Constants;
using Gymify.Domain.RepositoriesContracts;

namespace Gymify.Application.Features.Customers.Commands.UpdateCustomer
{
    public class UpdateCustomerCommandHandler : ICommandHandler<UpdateCustomerCommand, UpdateCustomerCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPhoneNumberService _phoneNumberService;
        private readonly IMapper _mapper;

        public UpdateCustomerCommandHandler(
            IUnitOfWork unitOfWork,
            IPhoneNumberService phoneNumberService,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _phoneNumberService = phoneNumberService;
            _mapper = mapper;
        }

        public async Task<ErrorOr<UpdateCustomerCommandResponse>> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
        {
            var customer = await _unitOfWork.Customers.FirstOrDefaultAsync(
                new CustomerWithDomainUserByIdSpec(request.CustomerId),
                cancellationToken);

            if (customer is null)
            {
                return Error.NotFound(
                    code: ErrorCodes.Customer.CustomerNotFound,
                    description: $"Customer with ID {request.CustomerId} was not found.");
            }

            string? normalizedPhone = null;
            if (request.PhoneNumber is not null)
            {
                normalizedPhone = _phoneNumberService.Normalize(request.PhoneNumber);
                var phoneExists = await _unitOfWork.Users.AnyAsync(
                    u => u.PhoneNumber == normalizedPhone && u.Id != customer.DomainUserId,
                    cancellationToken);

                if (phoneExists)
                {
                    return Error.Conflict(
                        code: ErrorCodes.User.PhoneAlreadyExists,
                        description: "Phone number already exists.");
                }
            }

            if (request.Name is not null || normalizedPhone is not null)
            {
                customer.DomainUser.UpdateInfo(request.Name, normalizedPhone);
            }

            if (request.IsActive.HasValue)
            {
                customer.DomainUser.SetActive(request.IsActive.Value);
            }

            customer.UpdateDetails(
                height: request.Height,
                weight: request.Weight,
                code: request.Code,
                age: request.Age,
                injuries: request.Injuries,
                chronicDiseases: request.ChronicDiseases,
                musclePercentage: request.MusclePercentage,
                fatPercentage: request.FatPercentage,
                notes: request.Notes,
                approvalStatus: request.ApprovalStatus);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return _mapper.Map<UpdateCustomerCommandResponse>(customer);
        }
    }
}
