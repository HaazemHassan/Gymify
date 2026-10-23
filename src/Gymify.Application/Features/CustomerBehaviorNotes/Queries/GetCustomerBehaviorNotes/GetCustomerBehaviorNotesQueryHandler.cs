using ErrorOr;
using Gymify.Application.Common.CQRS;
using Gymify.Application.Common.Pagination;
using Gymify.Application.Features.CustomerBehaviorNotes.Queries.GetCustomerBehaviorNotes.Specifications;
using Gymify.Domain.RepositoriesContracts;

namespace Gymify.Application.Features.CustomerBehaviorNotes.Queries.GetCustomerBehaviorNotes
{
    public class GetCustomerBehaviorNotesQueryHandler : IQueryHandler<GetCustomerBehaviorNotesQuery, PaginatedResult<GetCustomerBehaviorNotesQueryResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetCustomerBehaviorNotesQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ErrorOr<PaginatedResult<GetCustomerBehaviorNotesQueryResponse>>> Handle(GetCustomerBehaviorNotesQuery request, CancellationToken cancellationToken)
        {
            var paginatedSpec = new GetCustomerBehaviorNotesPaginatedSpec(request.PageNumber, request.PageSize, request.CustomerId, request.Search);
            var countSpec = new GetCustomerBehaviorNotesCountSpec(request.CustomerId, request.Search);

            var items = await _unitOfWork.CustomerBehaviorNotes.ListAsync<GetCustomerBehaviorNotesQueryResponse>(paginatedSpec, cancellationToken);
            var totalCount = await _unitOfWork.CustomerBehaviorNotes.CountAsync(countSpec, cancellationToken);

            return new PaginatedResult<GetCustomerBehaviorNotesQueryResponse>(items, totalCount, request.PageNumber, request.PageSize);
        }
    }
}
