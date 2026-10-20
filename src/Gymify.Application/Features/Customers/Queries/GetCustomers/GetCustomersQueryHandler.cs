using ErrorOr;
using Gymify.Application.Common.CQRS;
using Gymify.Application.Common.Pagination;
using Gymify.Application.Features.Customers.Queries.GetCustomers.Specifications;
using Gymify.Domain.RepositoriesContracts;

namespace Gymify.Application.Features.Customers.Queries.GetCustomers
{
    public class GetCustomersQueryHandler : IQueryHandler<GetCustomersQuery, PaginatedResult<GetCustomersQueryResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetCustomersQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ErrorOr<PaginatedResult<GetCustomersQueryResponse>>> Handle(GetCustomersQuery request, CancellationToken cancellationToken)
        {
            var filterSpec = new CustomersPaginatedSpec(
                request.PageNumber,
                request.PageSize,
                request.Search,
                request.ApprovalStatus);

            var countSpec = new CustomersCountSpec(
                request.Search,
                request.ApprovalStatus);

            var items = await _unitOfWork.Customers.ListAsync<GetCustomersQueryResponse>(filterSpec, cancellationToken);
            var totalCount = await _unitOfWork.Customers.CountAsync(countSpec, cancellationToken);

            return new PaginatedResult<GetCustomersQueryResponse>(items, totalCount, request.PageNumber, request.PageSize);
        }
    }
}
