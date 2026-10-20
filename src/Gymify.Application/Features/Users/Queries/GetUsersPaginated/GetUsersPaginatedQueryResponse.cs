using Gymify.Application.Features.Users.Common;

namespace Gymify.Application.Features.Users.Queries.GetUsersPaginated
{
    public class GetUsersPaginatedQueryResponse : UserBaseResponse
    {

        public string? Address { get; set; }
        public string Phone { get; set; } = string.Empty;

    }
}
