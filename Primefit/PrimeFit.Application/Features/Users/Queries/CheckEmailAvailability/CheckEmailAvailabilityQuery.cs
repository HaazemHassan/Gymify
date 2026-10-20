
namespace PrimeFit.Application.Features.Users.Queries.CheckEmailAvailability {
    public class CheckEmailAvailabilityQuery : IQuery<CheckEmailAvailabilityQueryResponse> {
        public string Email { get; set; }
    }
}
