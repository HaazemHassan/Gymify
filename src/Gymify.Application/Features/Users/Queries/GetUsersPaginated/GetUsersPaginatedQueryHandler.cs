//using MediatR;
//using Gymify.Application.Common.Pagination;
//using Gymify.Application.Specifications.Users;
//using Gymify.Domain.Repositories;
//using Gymify.Domain.Specifications.Users;

//namespace Gymify.Application.Features.Users.Queries.GetUsersPaginated
//{
//    public class GetUsersPaginatedQueryHandler : IRequestHandler<GetUsersPaginatedQuery, PaginatedResult<GetUsersPaginatedQueryResponse>>
//    {
//        private readonly IUnitOfWork _unitOfWork;

//        public GetUsersPaginatedQueryHandler(IUnitOfWork unitOfWork)
//        {
//            _unitOfWork = unitOfWork;
//        }

//        public async Task<PaginatedResult<GetUsersPaginatedQueryResponse>> Handle(GetUsersPaginatedQuery request, CancellationToken cancellationToken)
//        {

//            var dataSpec = new UsersPaginatedSpec(request.PageNumber, request.PageSize, request.Search, request.SortBy);
//            var countSpec = new UsersSearchSpec(request.Search);

//            var items = await _unitOfWork.Users.ListAsync<GetUsersPaginatedQueryResponse>(dataSpec, cancellationToken);
//            var totalCount = await _unitOfWork.Users.CountAsync(countSpec, cancellationToken);

//            return new PaginatedResult<GetUsersPaginatedQueryResponse>(items, totalCount, request.PageNumber, request.PageSize);
//        }


//    }
//}
