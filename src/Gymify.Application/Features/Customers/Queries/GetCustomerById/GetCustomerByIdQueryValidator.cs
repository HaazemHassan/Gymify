using FluentValidation;
using Gymify.Application.Features.Customers.Shared;

namespace Gymify.Application.Features.Customers.Queries.GetCustomerById
{
    public class GetCustomerByIdQueryValidator : AbstractValidator<GetCustomerByIdQuery>
    {
        public GetCustomerByIdQueryValidator()
        {
            ApplyValidationRules();
        }

        private void ApplyValidationRules()
        {
            RuleFor(x => x.Id).ApplyCustomerIdRules();
        }
    }
}
