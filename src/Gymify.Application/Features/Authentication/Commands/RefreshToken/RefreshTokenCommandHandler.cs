using Gymify.Application.Features.Authentication.Common;
using Gymify.Application.ServicesContracts.Infrastructure;
using Gymify.Domain.RepositoriesContracts;

namespace Gymify.Application.Features.Authentication.Commands.RefreshToken;

public class RefreshTokenCommandHandler(
    IAuthenticationService _authenticationService,
    IApplicationUserService _applicationUserService,
    IUnitOfWork _unitOfWork)
    : ICommandHandler<RefreshTokenCommand, AuthResponse>
{
    public async Task<ErrorOr<AuthResponse>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var authResult = await _authenticationService.ReAuthenticateAsync(request.RefreshToken!, request.AccessToken, cancellationToken);
        if (authResult.IsError)
        {
            return authResult.Errors;
        }

        var userProfileResult = await _applicationUserService.GetUserAuthProfileAsync(authResult.Value.DomainUserId, cancellationToken);
        if (userProfileResult.IsError)
        {
            return userProfileResult.Errors;
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new AuthResponse(authResult.Value, userProfileResult.Value);
    }
}
