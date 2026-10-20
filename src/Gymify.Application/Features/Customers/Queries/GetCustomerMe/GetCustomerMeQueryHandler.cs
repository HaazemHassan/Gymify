using Gymify.Application.Contracts.Api;
using Gymify.Application.Features.Customers.Queries.GetCustomerMe.Specifications;
using Gymify.Domain.Common.Constants;
using Gymify.Domain.RepositoriesContracts;

namespace Gymify.Application.Features.Customers.Queries.GetCustomerMe
{
    public class GetCustomerMeQueryHandler : IQueryHandler<GetCustomerMeQuery, GetCustomerMeQueryResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public GetCustomerMeQueryHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task<ErrorOr<GetCustomerMeQueryResponse>> Handle(GetCustomerMeQuery request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId;


            var spec = new CustomerByDomainUserIdSpec(userId!.Value);
            var customerResponse = await _unitOfWork.Customers.GetAsync<GetCustomerMeQueryResponse>(spec, cancellationToken);

            if (customerResponse is null)
            {
                return Error.NotFound(code: ErrorCodes.Customer.CustomerNotFound, description: "Customer profile not found");
            }

            return customerResponse;
        }
    }
}
