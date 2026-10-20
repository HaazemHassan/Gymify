using AutoMapper;
using Gymify.Application.Features.Users.Common;
using Gymify.Application.ServicesContracts.Infrastructure;
using Gymify.Domain.Common.Constants;
using Gymify.Domain.Common.Enums;
using Gymify.Domain.Entities;
using Gymify.Domain.RepositoriesContracts;

namespace Gymify.Application.Features.Authentication.Commands.RegisterCustomer
{
    public class RegisterCustomerCommandHandler : ICommandHandler<RegisterCustomerCommand, RegisterCustomerResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IApplicationUserService _applicationUserService;
        private readonly IPhoneNumberService _phoneNumberService;


        public RegisterCustomerCommandHandler(IUnitOfWork unitOfWork,
            IMapper mapper,
            IApplicationUserService applicationUserService,
            IPhoneNumberService phoneNumberService
            )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _applicationUserService = applicationUserService;
            _phoneNumberService = phoneNumberService;
        }

        public async Task<ErrorOr<RegisterCustomerResponse>> Handle(RegisterCustomerCommand request, CancellationToken cancellationToken)
        {
            string normalizedPhone = _phoneNumberService.Normalize(request.PhoneNumber);


            var existingDomainUser = await _unitOfWork.Users.GetAsync(u => u.Email == request.Email, cancellationToken);

            DomainUser domainUser;

            if (existingDomainUser is not null)
            {
                var isConfirmed = await _applicationUserService.IsEmailConfirmedAsync(existingDomainUser.Id, cancellationToken);
                if (isConfirmed)
                {
                    return Error.Conflict(
                        code: ErrorCodes.User.EmailAlreadyExists,
                        description: "An account with this email already exists.");
                }



                var isPhoneExists = await _unitOfWork.Users.AnyAsync(u => u.PhoneNumber == normalizedPhone && u.Id != existingDomainUser.Id, cancellationToken);
                if (isPhoneExists)
                {
                    return Error.Conflict(
                        code: ErrorCodes.User.PhoneAlreadyExists,
                        description: "Phone number already exists");
                }


                existingDomainUser.UpdateInfo(request.Name, normalizedPhone);
                domainUser = existingDomainUser;

                var updateResult = await _applicationUserService.UpdateUnconfirmedUserAsync(domainUser, request.Password, cancellationToken);
                if (updateResult.IsError)
                {
                    return updateResult.Errors;
                }
            }
            else
            {

                var phoneExists = await _unitOfWork.Users.AnyAsync(u => u.PhoneNumber == normalizedPhone, cancellationToken);

                if (phoneExists)
                {
                    return Error.Conflict(
                        code: ErrorCodes.User.PhoneAlreadyExists,
                        description: "Phone number already exists");
                }


                domainUser = new DomainUser(
                    UserType.Customer,
                    request.Name,
                    request.Email,
                    normalizedPhone
                );

                var addUserResult = await _applicationUserService.AddUser(domainUser, request.Password, ct: cancellationToken);
                if (addUserResult.IsError)
                {
                    return addUserResult.Errors;
                }
            }

            var customer = await _unitOfWork.Customers.GetAsync(c => c.DomainUserId == domainUser.Id, cancellationToken);
            if (customer is null)
            {
                customer = new Customer(domainUser.Id, approvalStatus: ApprovalStatus.Pending);
                await _unitOfWork.Customers.AddAsync(customer, cancellationToken);
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var response = _mapper.Map<RegisterCustomerResponse>(domainUser);
            response.ApprovalStatus = customer.ApprovalStatus;

            return response;
        }
    }
}
