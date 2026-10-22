using ErrorOr;
using Gymify.Application.Common.CQRS;
using Gymify.Application.Common.Pagination;
using Gymify.Application.Features.Trainers.Queries.GetTrainersForManagement.Specifications;
using Gymify.Application.Features.Trainers.Specifications;
using Gymify.Domain.RepositoriesContracts;

namespace Gymify.Application.Features.Trainers.Queries.GetTrainersForManagement
{
    public class GetTrainersForManagementQueryHandler : IQueryHandler<GetTrainersForManagementQuery, PaginatedResult<GetTrainersForManagementQueryResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetTrainersForManagementQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ErrorOr<PaginatedResult<GetTrainersForManagementQueryResponse>>> Handle(GetTrainersForManagementQuery request, CancellationToken cancellationToken)
        {
            var dataSpec = new TrainersForManagementPaginatedSpec(request.PageNumber, request.PageSize, request.Search, request.IsActive);
            var countSpec = new TrainersFilteredSpec(request.Search, isActive: request.IsActive);

            var trainers = await _unitOfWork.Trainers.ListAsync<GetTrainersForManagementQueryResponse>(dataSpec, cancellationToken);
            var totalCount = await _unitOfWork.Trainers.CountAsync(countSpec, cancellationToken);

            return new PaginatedResult<GetTrainersForManagementQueryResponse>(trainers, totalCount, request.PageNumber, request.PageSize);
        }
    }
}
