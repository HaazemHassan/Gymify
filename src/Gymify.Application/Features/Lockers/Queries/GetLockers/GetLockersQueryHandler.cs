using ErrorOr;
using Gymify.Application.Common.CQRS;
using Gymify.Application.Features.Lockers.Queries.GetLockers.Specifications;
using Gymify.Domain.RepositoriesContracts;

namespace Gymify.Application.Features.Lockers.Queries.GetLockers
{
    public class GetLockersQueryHandler : IQueryHandler<GetLockersQuery, List<GetLockersQueryResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetLockersQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ErrorOr<List<GetLockersQueryResponse>>> Handle(GetLockersQuery request, CancellationToken cancellationToken)
        {
            var spec = new GetLockersSpec(request.IsActive, request.Search);
            var lockers = await _unitOfWork.Lockers.ListAsync<GetLockersQueryResponse>(spec, cancellationToken);

            return lockers;
        }
    }
}
