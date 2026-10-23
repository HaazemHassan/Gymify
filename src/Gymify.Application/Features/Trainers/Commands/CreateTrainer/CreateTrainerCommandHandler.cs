using ErrorOr;
using Gymify.Application.Common.CQRS;
using Gymify.Application.ServicesContracts.Infrastructure;
using Gymify.Domain.Common.Constants;
using Gymify.Domain.Common.Enums;
using Gymify.Domain.Entities;
using Gymify.Domain.RepositoriesContracts;

namespace Gymify.Application.Features.Trainers.Commands.CreateTrainer
{
    public class CreateTrainerCommandHandler : ICommandHandler<CreateTrainerCommand, CreateTrainerCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IApplicationUserService _applicationUserService;
        private readonly IPhoneNumberService _phoneNumberService;

        public CreateTrainerCommandHandler(
            IUnitOfWork unitOfWork,
            IApplicationUserService applicationUserService,
            IPhoneNumberService phoneNumberService)
        {
            _unitOfWork = unitOfWork;
            _applicationUserService = applicationUserService;
            _phoneNumberService = phoneNumberService;
        }

        public async Task<ErrorOr<CreateTrainerCommandResponse>> Handle(CreateTrainerCommand request, CancellationToken cancellationToken)
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

            var domainUser = new DomainUser(
                UserType.Employee,
                request.Name,
                request.Email,
                normalizedPhone,
                request.IsActive);

            var addUserResult = await _applicationUserService.AddUser(domainUser, request.Password, cancellationToken);
            if (addUserResult.IsError)
            {
                return addUserResult.Errors;
            }

            var assignRoleResult = await _applicationUserService.AssignRoleAsync(domainUser.Id, UserRole.Trainer, cancellationToken);
            if (assignRoleResult.IsError)
            {
                return assignRoleResult.Errors;
            }

            var employee = new Employee(
                domainUserId: domainUser.Id,
                salary: request.Salary,
                isActive: request.IsActive);

            var trainer = new Trainer(
                employee: employee,
                yearsOfExperience: request.YearsOfExperience,
                isActive: request.IsActive);

            await _unitOfWork.Employees.AddAsync(employee, cancellationToken);
            await _unitOfWork.Trainers.AddAsync(trainer, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new CreateTrainerCommandResponse(trainer.Id, employee.Id, domainUser.Id);
        }
    }
}
