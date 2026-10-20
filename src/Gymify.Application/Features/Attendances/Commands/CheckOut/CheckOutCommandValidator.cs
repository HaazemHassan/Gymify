using FluentValidation;
using Gymify.Application.Features.Attendances.Shared;

namespace Gymify.Application.Features.Attendances.Commands.CheckOut
{
    public class CheckOutCommandValidator : AbstractValidator<CheckOutCommand>
    {
        public CheckOutCommandValidator()
        {
            ApplyValidationRules();
        }

        private void ApplyValidationRules()
        {
            RuleFor(x => x.Code)
                .ApplyCustomerCodeRules();
        }
    }
}
