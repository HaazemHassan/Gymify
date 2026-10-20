using FluentValidation;
using Gymify.Application.Features.Customers.Shared;

namespace Gymify.Application.Features.Customers.Commands.ChangeCustomerApprovalStatus
{
    public class ChangeCustomerApprovalStatusCommandValidator : AbstractValidator<ChangeCustomerApprovalStatusCommand>
    {
        public ChangeCustomerApprovalStatusCommandValidator()
        {
            ApplyValidationRules();
        }

        private void ApplyValidationRules()
        {
            RuleFor(x => x.CustomerId).ApplyCustomerIdRules();
            RuleFor(x => x.Status).ApplyApprovalStatusRules();
        }
    }
}
