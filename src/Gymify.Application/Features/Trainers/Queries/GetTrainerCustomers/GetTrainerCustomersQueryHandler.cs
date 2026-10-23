using ErrorOr;
using Gymify.Application.Common.CQRS;
using Gymify.Application.Common.Pagination;
using Gymify.Application.Features.Trainers.Queries.GetTrainerCustomers.Specifications;
using Gymify.Application.ServicesContracts.Infrastructure;
using Gymify.Domain.Common.Constants;
using Gymify.Domain.RepositoriesContracts;

namespace Gymify.Application.Features.Trainers.Queries.GetTrainerCustomers
{
    public class GetTrainerCustomersQueryHandler : IQueryHandler<GetTrainerCustomersQuery, PaginatedResult<GetTrainerCustomersQueryResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDateTimeProvider _dateTimeProvider;

        public GetTrainerCustomersQueryHandler(
            IUnitOfWork unitOfWork,
            IDateTimeProvider dateTimeProvider)
        {
            _unitOfWork = unitOfWork;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<ErrorOr<PaginatedResult<GetTrainerCustomersQueryResponse>>> Handle(GetTrainerCustomersQuery request, CancellationToken cancellationToken)
        {
            var trainerExists = await _unitOfWork.Trainers.AnyAsync(t => t.Id == request.TrainerId, cancellationToken);
            if (!trainerExists)
            {
                return Error.NotFound(
                    code: ErrorCodes.Trainer.TrainerNotFound,
                    description: "Trainer profile not found.");
            }

            var now = _dateTimeProvider.UtcNow;
            var paginatedSpec = new TrainerCustomersPaginatedSpec(request.TrainerId, now, request.PageNumber, request.PageSize, request.Search);
            var countSpec = new TrainerCustomersCountSpec(request.TrainerId, now, request.Search);

            var items = await _unitOfWork.Subscriptions.ListAsync<GetTrainerCustomersQueryResponse>(paginatedSpec, cancellationToken);
            var totalCount = await _unitOfWork.Subscriptions.CountAsync(countSpec, cancellationToken);

            return new PaginatedResult<GetTrainerCustomersQueryResponse>(items, totalCount, request.PageNumber, request.PageSize);
        }
    }
}
