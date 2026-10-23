using Gymify.Application.ServicesContracts.Infrastructure;
using Gymify.Domain.Common.Constants;
using Gymify.Domain.Common.Enums;
using Gymify.Domain.Entities;
using Gymify.Domain.RepositoriesContracts;

namespace Gymify.Application.Features.Customers.Commands.CreateCustomer
{
    public class CreateCustomerCommandHandler : ICommandHandler<CreateCustomerCommand, CreateCustomerCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IApplicationUserService _applicationUserService;
        private readonly IPhoneNumberService _phoneNumberService;

        public CreateCustomerCommandHandler(
            IUnitOfWork unitOfWork,
            IApplicationUserService applicationUserService,
            IPhoneNumberService phoneNumberService)
        {
            _unitOfWork = unitOfWork;
            _applicationUserService = applicationUserService;
            _phoneNumberService = phoneNumberService;
        }

        public async Task<ErrorOr<CreateCustomerCommandResponse>> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
        {
            string normalizedPhone = _phoneNumberService.Normalize(request.PhoneNumber);

            var emailExists = await _unitOfWork.Users.AnyAsync(u => u.Email == request.Email, cancellationToken);
            if (emailExists)
            {
                return Error.Conflict(
                    code: ErrorCodes.User.EmailAlreadyExists,
                    description: "An account with this email already exists.");
            }

            var phoneExists = await _unitOfWork.Users.AnyAsync(u => u.PhoneNumber == normalizedPhone, cancellationToken);
            if (phoneExists)
            {
                return Error.Conflict(
                    code: ErrorCodes.User.PhoneAlreadyExists,
                    description: "Phone number already exists.");
            }

            if (!string.IsNullOrWhiteSpace(request.Code))
            {
                var codeExists = await _unitOfWork.Customers.AnyAsync(c => c.Code == request.Code, cancellationToken);
                if (codeExists)
                {
                    return Error.Conflict(
                        code: ErrorCodes.Customer.CodeAlreadyExists,
                        description: "Customer code already exists.");
                }
            }

            var domainUser = DomainUser.Create(
                UserType.Customer,
                request.Name,
                request.Email,
                normalizedPhone,
                request.IsActive);

            var addUserResult = await _applicationUserService.AddUser(
                domainUser,
                request.Password,
                emailConfirmed: true,
                cancellationToken);

            if (addUserResult.IsError)
            {
                return addUserResult.Errors;
            }

            var assignRoleResult = await _applicationUserService.AssignRoleAsync(domainUser.Id, UserRole.Customer, cancellationToken);
            if (assignRoleResult.IsError)
            {
                return assignRoleResult.Errors;
            }

            var customer = new Customer(
                domainUserId: domainUser.Id,
                height: request.Height,
                weight: request.Weight,
                approvalStatus: ApprovalStatus.Approved,
                age: request.Age,
                code: request.Code,
                injuries: request.Injuries,
                chronicDiseases: request.ChronicDiseases,
                musclePercentage: request.MusclePercentage,
                fatPercentage: request.FatPercentage,
                notes: request.Notes);

            await _unitOfWork.Customers.AddAsync(customer, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new CreateCustomerCommandResponse(customer.Id, domainUser.Id);
        }
    }
}
