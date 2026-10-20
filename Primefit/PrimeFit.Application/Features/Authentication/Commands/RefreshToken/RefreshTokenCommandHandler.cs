using PrimeFit.Application.Features.Authentication.Common;
using PrimeFit.Application.ServicesContracts.Infrastructure;
using PrimeFit.Domain.RepositoriesContracts;

namespace PrimeFit.Application.Features.Authentication.Commands.RefreshToken;

public class RefreshTokenCommandHandler(IAuthenticationService _authenticationService, IUnitOfWork _unitOfWork)
    : ICommandHandler<RefreshTokenCommand, AuthResult>
{

    public async Task<ErrorOr<AuthResult>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var authResult = await _authenticationService.ReAuthenticateAsync(request.RefreshToken!, request.AccessToken);
        if (!authResult.IsError)
        {
            await _unitOfWork.SaveChangesAsync(cancellationToken);

        }
        return authResult;
    }
}
