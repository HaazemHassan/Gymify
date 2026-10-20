using Gymify.Application.Common.Transaction;
using Gymify.Application.Features.Authentication.Common;
using Gymify.Domain.Common.Enums;

namespace Gymify.Application.Features.Authentication.Commands.SignInWithGoogle
{
    public class SignInWithGoogleCommand : ICommand<AuthResponse>, ITransactionalRequest
    {
        public string IdToken { get; set; } = string.Empty;
        public UserType UserType { get; set; }
    }
}
