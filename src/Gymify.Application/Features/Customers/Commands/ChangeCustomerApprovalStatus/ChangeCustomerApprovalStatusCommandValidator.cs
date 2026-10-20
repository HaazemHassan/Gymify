using FluentValidation;

namespace Gymify.Application.Features.Customers.Commands.ChangeCustomerApprovalStatus
{
    public class ChangeCustomerApprovalStatusCommandValidator : AbstractValidator<ChangeCustomerApprovalStatusCommand>
    {
        public ChangeCustomerApprovalStatusCommandValidator()
        {
            RuleFor(x => x.CustomerId)
                .GreaterThan(0)
                .WithMessage("Customer ID must be greater than zero.");

            RuleFor(x => x.Status)
                .IsInEnum()
                .WithMessage("Invalid approval status.");
        }
    }
}
