using PrimeFit.Application.Common.Pagination;

namespace PrimeFit.Application.Features.BranchReviews.Queries.GetBranchReviews
{
    public class GetBranchReviewsQuery : PaginatedQuery, IQuery<PaginatedResult<GetBranchReviewsQueryResponse>>
    {
        public int BranchId { get; set; }
        public int? Rating { get; set; }
    }
}
