using Gymify.Application.Contracts.Api;
using Gymify.Application.Contracts.Infrastructure;
using Gymify.Domain.RepositoriesContracts;

namespace Gymify.Application.Features.Authentication.Commands.SendResetPasswordEmail
{
    internal class SendResetPasswordEmailCommandHandler : ICommandHandler<SendResetPasswordEmailCommand>
    {
        private readonly IPasswordService _passwordService;
        private readonly IUnitOfWork _unitOfWork;

        public SendResetPasswordEmailCommandHandler(IPasswordService passwordService, IUnitOfWork unitOfWork)
        {
            _passwordService = passwordService;
            _unitOfWork = unitOfWork;
        }

        public async Task<ErrorOr<Success>> Handle(SendResetPasswordEmailCommand request, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.Users.GetAsync(u => u.Email == request.Email, cancellationToken);
            if (user is null)
            {
                return Result.Success;
            }

            var result = await _passwordService.CreatePasswordResetCode(user.Id, cancellationToken);

            if (result.IsError)
            {
                return result.Errors;
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            await _passwordService.SendPasswordResetEmailAsync(user, result.Value);

            return Result.Success;
        }
    }
}
