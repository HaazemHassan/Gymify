using PrimeFit.Application.Security;
using PrimeFit.Application.Security.Markers;
using PrimeFit.Application.Security.Policies;

namespace PrimeFit.Application.Features.Users.Commands.UpdateProfile
{
    [Authorize(Policy = AuthorizationPolicies.SelfOnly)]
    public class UpdateProfileCommand : ICommand<UpdateProfileCommandResponse>, IOwnedResourceRequest
    {
        public int UserId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? PhoneNumber { get; set; }


    }
}
