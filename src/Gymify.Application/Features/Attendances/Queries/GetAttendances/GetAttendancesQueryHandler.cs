using ErrorOr;
using Gymify.Application.Common.CQRS;
using Gymify.Application.Common.Pagination;
using Gymify.Application.Features.Attendances.Queries.GetAttendances.Specifications;
using Gymify.Application.Features.Attendances.Specifications;
using Gymify.Domain.RepositoriesContracts;

namespace Gymify.Application.Features.Attendances.Queries.GetAttendances
{
    public class GetAttendancesQueryHandler : IQueryHandler<GetAttendancesQuery, PaginatedResult<GetAttendancesQueryResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAttendancesQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ErrorOr<PaginatedResult<GetAttendancesQueryResponse>>> Handle(GetAttendancesQuery request, CancellationToken cancellationToken)
        {
            var dataSpec = new AttendancesPaginatedSpec(
                request.PageNumber,
                request.PageSize,
                request.Search,
                request.IsCurrentlyAttending,
                request.FromDate);

            var countSpec = new AttendancesFilteredSpec(
                request.Search,
                request.IsCurrentlyAttending,
                request.FromDate);

            var attendances = await _unitOfWork.Attendances.ListAsync<GetAttendancesQueryResponse>(dataSpec, cancellationToken);
            var totalCount = await _unitOfWork.Attendances.CountAsync(countSpec, cancellationToken);

            return new PaginatedResult<GetAttendancesQueryResponse>(attendances, totalCount, request.PageNumber, request.PageSize);
        }
    }
}
