using AutoMapper;
using Gymify.Application.ServicesContracts.Infrastructure;
using Gymify.Domain.Common.Constants;
using Gymify.Domain.Entities;
using Gymify.Domain.RepositoriesContracts;

namespace Gymify.Application.Features.Users.Commands.UpdateProfile
{
    public class UpdateProfileCommandHandler : ICommandHandler<UpdateProfileCommand, UpdateProfileCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IPhoneNumberService _phoneNumberService;

        public UpdateProfileCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IPhoneNumberService phoneNumberService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _phoneNumberService = phoneNumberService;
        }

        public async Task<ErrorOr<UpdateProfileCommandResponse>> Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
        {
            string? normalizedPhoneNumber = null;

            var userFromDb = await _unitOfWork.Users.GetByIdAsync(request.UserId, cancellationToken);
            if (userFromDb is null)
            {
                return Error.NotFound(description: "User not found");

            }

            if (request.PhoneNumber is not null)
            {
                normalizedPhoneNumber = _phoneNumberService.Normalize(request.PhoneNumber);
                var isPhoneExists = await _unitOfWork.Users.AnyAsync(u =>
                            u.PhoneNumber == normalizedPhoneNumber
                           && u.Id != userFromDb.Id, cancellationToken);

                if (isPhoneExists)
                {
                    return Error.Conflict(code: ErrorCodes.User.PhoneAlreadyExists, description: "Phone number already exists");

                }

            }

            userFromDb.UpdateInfo(
                name: request.Name,
                phoneNumber: normalizedPhoneNumber
            );

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var userResponse = _mapper.Map<DomainUser, UpdateProfileCommandResponse>(userFromDb);
            return userResponse;
        }
    }
}

