using ErrorOr;
using Gymify.Application.Common.CQRS;
using Gymify.Application.Features.Customers.Queries.GetCustomerById.Specifications;
using Gymify.Domain.Common.Constants;
using Gymify.Domain.RepositoriesContracts;

namespace Gymify.Application.Features.Customers.Queries.GetCustomerById
{
    public class GetCustomerByIdQueryHandler : IQueryHandler<GetCustomerByIdQuery, GetCustomerByIdQueryResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetCustomerByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ErrorOr<GetCustomerByIdQueryResponse>> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
        {
            var spec = new CustomerDetailsByIdSpec(request.Id);
            var customer = await _unitOfWork.Customers.GetAsync<GetCustomerByIdQueryResponse>(spec, cancellationToken);

            if (customer is null)
            {
                return Error.NotFound(code: ErrorCodes.Customer.CustomerNotFound, description: "Customer not found.");
            }

            return customer;
        }
    }
}
