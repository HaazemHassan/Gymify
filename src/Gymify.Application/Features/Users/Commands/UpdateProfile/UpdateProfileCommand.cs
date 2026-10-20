using Gymify.Application.Security;
using Gymify.Application.Security.Markers;
using Gymify.Application.Security.Policies;

namespace Gymify.Application.Features.Users.Commands.UpdateProfile
{
    [Authorize(Policy = AuthorizationPolicies.SelfOnly)]
    public class UpdateProfileCommand : ICommand<UpdateProfileCommandResponse>, IOwnedResourceRequest
    {
        public int UserId { get; set; }
        public string? Name { get; set; }
        public string? PhoneNumber { get; set; }


    }
}
