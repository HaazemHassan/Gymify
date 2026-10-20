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

        public UpdateCustomerCommandHandler(IUnitOfWork unitOfWork, IPhoneNumberService phoneNumberService)
        {
            _unitOfWork = unitOfWork;
            _phoneNumberService = phoneNumberService;
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

            return new UpdateCustomerCommandResponse
            {
                Id = customer.Id,
                CustomerId = customer.Id,
                UserId = customer.DomainUserId,
                Name = customer.DomainUser.Name,
                Email = customer.DomainUser.Email,
                PhoneNumber = customer.DomainUser.PhoneNumber,
                IsActive = customer.DomainUser.IsActive,
                Code = customer.Code,
                Height = customer.Height,
                Weight = customer.Weight,
                Age = customer.Age,
                Injuries = customer.Injuries,
                ChronicDiseases = customer.ChronicDiseases,
                MusclePercentage = customer.MusclePercentage,
                FatPercentage = customer.FatPercentage,
                Notes = customer.Notes,
                ApprovalStatus = customer.ApprovalStatus
            };
        }
    }
}
