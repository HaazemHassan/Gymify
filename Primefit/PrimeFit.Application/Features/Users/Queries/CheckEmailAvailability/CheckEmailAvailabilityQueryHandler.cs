using PrimeFit.Domain.RepositoriesContracts;

namespace PrimeFit.Application.Features.Users.Queries.CheckEmailAvailability
{
    public class CheckEmailAvailabilityQueryHandler(IUnitOfWork _unitOfWork)
        : IQueryHandler<CheckEmailAvailabilityQuery, CheckEmailAvailabilityQueryResponse>
    {

        public async Task<ErrorOr<CheckEmailAvailabilityQueryResponse>> Handle(CheckEmailAvailabilityQuery request, CancellationToken cancellationToken)
        {
            var emailExists = await _unitOfWork.Users.AnyAsync(u => u.Email == request.Email, cancellationToken);
            var response = new CheckEmailAvailabilityQueryResponse
            {
                IsAvailable = !emailExists
            };

            return response;
        }
    }
}
