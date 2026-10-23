using FluentValidation;

namespace Gymify.Application.Features.Subscriptions.Queries.GetFinancialSummary
{
    public class GetFinancialSummaryQueryValidator : AbstractValidator<GetFinancialSummaryQuery>
    {
        public GetFinancialSummaryQueryValidator()
        {
            ApplyValidationRules();
        }

        private void ApplyValidationRules()
        {
            When(x => x.FromDate.HasValue && x.ToDate.HasValue, () =>
            {
                RuleFor(x => x.ToDate)
                    .GreaterThanOrEqualTo(x => x.FromDate)
                    .WithMessage("ToDate must be greater than or equal to FromDate.");
            });
        }
    }
}
