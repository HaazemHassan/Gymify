using Gymify.Application.Common.Pagination;

namespace Gymify.Application.Features.Users.Queries.GetUsersPaginated {
    public class GetUsersPaginatedQuery : PaginatedQuery, IRequest<PaginatedResult<GetUsersPaginatedQueryResponse>> {

    }
}
