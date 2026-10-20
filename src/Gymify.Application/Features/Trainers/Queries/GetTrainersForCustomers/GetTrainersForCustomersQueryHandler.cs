using ErrorOr;
using Gymify.Application.Common.CQRS;
using Gymify.Application.Common.Pagination;
using Gymify.Application.Features.Trainers.Queries.GetTrainersForCustomers.Specifications;
using Gymify.Application.Features.Trainers.Specifications;
using Gymify.Domain.RepositoriesContracts;

namespace Gymify.Application.Features.Trainers.Queries.GetTrainersForCustomers
{
    public class GetTrainersForCustomersQueryHandler : IQueryHandler<GetTrainersForCustomersQuery, PaginatedResult<GetTrainersForCustomersQueryResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetTrainersForCustomersQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ErrorOr<PaginatedResult<GetTrainersForCustomersQueryResponse>>> Handle(GetTrainersForCustomersQuery request, CancellationToken cancellationToken)
        {
            var dataSpec = new TrainersForCustomersPaginatedSpec(request.PageNumber, request.PageSize, request.Search);
            var countSpec = new TrainersFilteredSpec(request.Search, isActive: true);

            var trainers = await _unitOfWork.Trainers.ListAsync<GetTrainersForCustomersQueryResponse>(dataSpec, cancellationToken);
            var totalCount = await _unitOfWork.Trainers.CountAsync(countSpec, cancellationToken);

            return new PaginatedResult<GetTrainersForCustomersQueryResponse>(trainers, totalCount, request.PageNumber, request.PageSize);
        }
    }
}
