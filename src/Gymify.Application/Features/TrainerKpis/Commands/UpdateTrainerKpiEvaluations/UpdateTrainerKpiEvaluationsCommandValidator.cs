using FluentValidation;
using Gymify.Application.Features.TrainerKpis.Shared;

namespace Gymify.Application.Features.TrainerKpis.Commands.UpdateTrainerKpiEvaluations
{
    public class UpdateTrainerKpiEvaluationsCommandValidator : AbstractValidator<UpdateTrainerKpiEvaluationsCommand>
    {
        public UpdateTrainerKpiEvaluationsCommandValidator()
        {
            ApplyValidationRules();
        }

        private void ApplyValidationRules()
        {
            RuleFor(x => x.TrainerId)
                .GreaterThan(0)
                .WithMessage("{PropertyName} must be greater than 0.");

            RuleFor(x => x.Date)
                .NotEmpty()
                .WithMessage("Date is required.");

            RuleFor(x => x.Scores)
                .NotEmpty()
                .WithMessage("At least one KPI score must be provided.")
                .Must(scores => scores.Select(s => s.KpiId).Distinct().Count() == scores.Count)
                .WithMessage("Duplicate KPI IDs found in submission.");

            RuleForEach(x => x.Scores).ChildRules(score =>
            {
                score.RuleFor(s => s.KpiId)
                    .GreaterThan(0)
                    .WithMessage("{PropertyName} must be greater than 0.");

                score.RuleFor(s => s.Score)
                    .ApplyKpiScoreRules();

                score.When(s => s.Comment != null, () =>
                {
                    score.RuleFor(s => s.Comment)
                        .ApplyCommentRules();
                });
            });
        }
    }
}
