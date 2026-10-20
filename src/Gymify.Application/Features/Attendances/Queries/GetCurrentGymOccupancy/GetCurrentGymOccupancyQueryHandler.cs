using ErrorOr;
using Gymify.Application.Common.CQRS;
using Gymify.Application.Features.Attendances.Queries.GetCurrentGymOccupancy.Specifications;
using Gymify.Domain.RepositoriesContracts;

namespace Gymify.Application.Features.Attendances.Queries.GetCurrentGymOccupancy
{
    public class GetCurrentGymOccupancyQueryHandler : IQueryHandler<GetCurrentGymOccupancyQuery, GetCurrentGymOccupancyQueryResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetCurrentGymOccupancyQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ErrorOr<GetCurrentGymOccupancyQueryResponse>> Handle(GetCurrentGymOccupancyQuery request, CancellationToken cancellationToken)
        {
            var spec = new ActiveAttendancesCountSpec();
            var count = await _unitOfWork.Attendances.CountAsync(spec, cancellationToken);

            return new GetCurrentGymOccupancyQueryResponse(count);
        }
    }
}
