using Gymify.Application.Features.Users.Common;

namespace Gymify.Application.Features.Users.Queries.GetUserById {
    public class GetUserByIdQueryResponse : UserBaseResponse {

        public string Address { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
    }
}
