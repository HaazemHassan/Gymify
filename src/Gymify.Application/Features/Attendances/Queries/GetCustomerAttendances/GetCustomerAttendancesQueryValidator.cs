using FluentValidation;
using Gymify.Application.Features.Customers.Shared;

namespace Gymify.Application.Features.Attendances.Queries.GetCustomerAttendances
{
    public class GetCustomerAttendancesQueryValidator : AbstractValidator<GetCustomerAttendancesQuery>
    {
        public GetCustomerAttendancesQueryValidator()
        {
            RuleFor(x => x.CustomerId)
                .ApplyCustomerIdRules();

            RuleFor(x => x.Month)
                .InclusiveBetween(1, 12)
                .When(x => x.Month.HasValue)
                .WithMessage("Month must be between 1 and 12.");

            RuleFor(x => x.Year)
                .GreaterThanOrEqualTo(2000)
                .When(x => x.Year.HasValue)
                .WithMessage("Year must be greater than or equal to 2000.");
        }
    }
}
