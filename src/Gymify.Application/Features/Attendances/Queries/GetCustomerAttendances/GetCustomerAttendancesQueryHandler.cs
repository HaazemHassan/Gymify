using ErrorOr;
using Gymify.Application.Common.CQRS;
using Gymify.Application.Features.Attendances.Queries.GetCustomerAttendances.Specifications;
using Gymify.Domain.Common.Constants;
using Gymify.Domain.RepositoriesContracts;

namespace Gymify.Application.Features.Attendances.Queries.GetCustomerAttendances
{
    public class GetCustomerAttendancesQueryHandler : IQueryHandler<GetCustomerAttendancesQuery, GetCustomerAttendancesQueryResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetCustomerAttendancesQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ErrorOr<GetCustomerAttendancesQueryResponse>> Handle(GetCustomerAttendancesQuery request, CancellationToken cancellationToken)
        {
            var customerExists = await _unitOfWork.Customers.AnyAsync(c => c.Id == request.CustomerId, cancellationToken);
            if (!customerExists)
            {
                return Error.NotFound(code: ErrorCodes.Customer.CustomerNotFound, description: "Customer not found.");
            }

            var now = DateTimeOffset.UtcNow;
            var targetYear = request.Year ?? now.Year;
            var targetMonth = request.Month ?? now.Month;

            var spec = new CustomerAttendancesSpec(request.CustomerId, targetMonth, targetYear);
            var attendances = await _unitOfWork.Attendances.ListAsync<CustomerAttendanceItemResponse>(spec, cancellationToken);

            return new GetCustomerAttendancesQueryResponse
            {
                CustomerId = request.CustomerId,
                Year = targetYear,
                Month = targetMonth,
                TotalAttendances = attendances.Count,
                Attendances = attendances
            };
        }
    }
}
