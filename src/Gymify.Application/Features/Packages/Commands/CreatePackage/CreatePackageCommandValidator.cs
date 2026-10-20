using FluentValidation;
using Gymify.Application.Common;
using Gymify.Application.Features.Packages.Shared;

namespace Gymify.Application.Features.Packages.Commands.CreatePackage
{
    public class CreatePackageCommandValidator : AbstractValidator<CreatePackageCommand>
    {
        public CreatePackageCommandValidator()
        {
            ApplyValidationRules();
        }

        private void ApplyValidationRules()
        {
            RuleFor(x => x.Name).Required();
            When(x => !string.IsNullOrWhiteSpace(x.Name), () =>
            {
                RuleFor(x => x.Name).ApplyPackageNameRules();
            });

            RuleFor(x => x.Price).ApplyPriceRules();
            RuleFor(x => x.Duration).ApplyDurationRules();

            When(x => !string.IsNullOrWhiteSpace(x.Description), () =>
            {
                RuleFor(x => x.Description).ApplyDescriptionRules();
            });
        }
    }
}
