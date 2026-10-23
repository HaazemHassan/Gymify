using FluentValidation;

namespace Gymify.Application.Features.CustomerBehaviorNotes.Commands.CreateCustomerBehaviorNote
{
    public class CreateCustomerBehaviorNoteCommandValidator : AbstractValidator<CreateCustomerBehaviorNoteCommand>
    {
        public CreateCustomerBehaviorNoteCommandValidator()
        {
            RuleFor(x => x.CustomerId)
                .GreaterThan(0);

            RuleFor(x => x.Description)
                .NotEmpty()
                .MaximumLength(2000);
        }
    }
}
