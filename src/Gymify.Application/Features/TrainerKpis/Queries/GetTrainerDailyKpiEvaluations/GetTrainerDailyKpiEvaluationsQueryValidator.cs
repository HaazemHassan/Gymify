using FluentValidation;

namespace Gymify.Application.Features.TrainerKpis.Queries.GetTrainerDailyKpiEvaluations
{
    public class GetTrainerDailyKpiEvaluationsQueryValidator : AbstractValidator<GetTrainerDailyKpiEvaluationsQuery>
    {
        public GetTrainerDailyKpiEvaluationsQueryValidator()
        {
            ApplyValidationRules();
        }

        private void ApplyValidationRules()
        {
            RuleFor(x => x.TrainerId)
                .GreaterThan(0)
                .WithMessage("{PropertyName} must be greater than 0.");

            When(x => x.From.HasValue && x.To.HasValue, () =>
            {
                RuleFor(x => x.From!.Value)
                    .LessThanOrEqualTo(x => x.To!.Value)
                    .WithMessage("'From' date must be less than or equal to 'To' date.");
            });
        }
    }
}
