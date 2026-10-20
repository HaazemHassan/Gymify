using Gymify.Application.Features.Authentication.Common;
using Gymify.Application.ServicesContracts.Infrastructure;
using Gymify.Domain.RepositoriesContracts;

namespace Gymify.Application.Features.Authentication.Commands.SignInWithGoogle
{
    internal sealed class SignInWithGoogleCommandHandler(
     IGoogleAuthService googleAuthService,
     IAuthenticationService authenticationService,
     IApplicationUserService applicationUserService,
     IUnitOfWork unitOfWork) : ICommandHandler<SignInWithGoogleCommand, AuthResponse>
    {
        public async Task<ErrorOr<AuthResponse>> Handle(SignInWithGoogleCommand request, CancellationToken cancellationToken)
        {
            var googleUserResult = await googleAuthService.ValidateIdTokenAsync(request.IdToken, cancellationToken);
            if (googleUserResult.IsError)
            {
                return googleUserResult.Errors;
            }

            var authResult = await authenticationService.SignInWithGoogleAsync(googleUserResult.Value, request.UserType, cancellationToken);
            if (authResult.IsError)
            {
                return authResult.Errors;
            }

            var userProfileResult = await applicationUserService.GetUserAuthProfileAsync(authResult.Value.DomainUserId, cancellationToken);
            if (userProfileResult.IsError)
            {
                return userProfileResult.Errors;
            }

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return new AuthResponse(authResult.Value, userProfileResult.Value);
        }
    }
}
