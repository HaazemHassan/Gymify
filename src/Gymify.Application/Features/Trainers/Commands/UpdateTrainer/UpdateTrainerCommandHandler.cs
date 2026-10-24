using AutoMapper;
using ErrorOr;
using Gymify.Application.Common.CQRS;
using Gymify.Application.Features.Trainers.Commands.UpdateTrainer.Specifications;
using Gymify.Application.ServicesContracts.Infrastructure;
using Gymify.Domain.Common.Constants;
using Gymify.Domain.RepositoriesContracts;

namespace Gymify.Application.Features.Trainers.Commands.UpdateTrainer
{
    public class UpdateTrainerCommandHandler : ICommandHandler<UpdateTrainerCommand, UpdateTrainerCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPhoneNumberService _phoneNumberService;
        private readonly IMapper _mapper;

        public UpdateTrainerCommandHandler(
            IUnitOfWork unitOfWork,
            IPhoneNumberService phoneNumberService,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _phoneNumberService = phoneNumberService;
            _mapper = mapper;
        }

        public async Task<ErrorOr<UpdateTrainerCommandResponse>> Handle(UpdateTrainerCommand request, CancellationToken cancellationToken)
        {
            var trainer = await _unitOfWork.Trainers.FirstOrDefaultAsync(
                new TrainerWithEmployeeAndUserByIdSpec(request.TrainerId),
                cancellationToken);

            if (trainer is null)
            {
                return Error.NotFound(
                    code: ErrorCodes.Trainer.TrainerNotFound,
                    description: "Trainer was not found.");
            }

            string? normalizedPhone = null;
            if (request.PhoneNumber is not null)
            {
                normalizedPhone = _phoneNumberService.Normalize(request.PhoneNumber);
                var phoneExists = await _unitOfWork.Users.AnyAsync(
                    u => u.PhoneNumber == normalizedPhone && u.Id != trainer.Employee.DomainUserId,
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
                trainer.Employee.DomainUser.UpdateInfo(request.Name, normalizedPhone);
            }

            if (request.IsActive.HasValue)
            {
                trainer.Employee.DomainUser.SetActive(request.IsActive.Value);
            }

            if (request.IsAvailable.HasValue)
            {
                trainer.SetAvailable(request.IsAvailable.Value);
            }

            if (request.YearsOfExperience.HasValue)
            {
                var updateExpResult = trainer.UpdateExperience(request.YearsOfExperience.Value);
                if (updateExpResult.IsError)
                {
                    return updateExpResult.Errors;
                }
            }

            if (request.Salary.HasValue)
            {
                trainer.Employee.UpdateSalary(request.Salary.Value);
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return _mapper.Map<UpdateTrainerCommandResponse>(trainer);
        }
    }
}
