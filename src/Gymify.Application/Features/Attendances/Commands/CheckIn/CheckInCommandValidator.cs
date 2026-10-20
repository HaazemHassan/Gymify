using FluentValidation;
using Gymify.Application.Features.Attendances.Shared;

namespace Gymify.Application.Features.Attendances.Commands.CheckIn
{
    public class CheckInCommandValidator : AbstractValidator<CheckInCommand>
    {
        public CheckInCommandValidator()
        {
            ApplyValidationRules();
        }

        private void ApplyValidationRules()
        {
            RuleFor(x => x.Code)
                .ApplyCustomerCodeRules();

            RuleFor(x => x.LockerId)
                .ApplyLockerIdRules();
        }
    }
}
