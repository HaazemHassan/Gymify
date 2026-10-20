using Gymify.Application.Features.Users.Common;

namespace Gymify.Application.Features.Users.Commands.UpdateProfile {
    public class UpdateProfileCommandResponse : UserBaseResponse {
        public string PhoneNumber { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
    }
}
