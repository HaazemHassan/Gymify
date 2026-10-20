
namespace PrimeFit.Application.Features.Subscriptions.Events
{
    public sealed record SubscriptionCreatedIntegrationEvent(int UserId, string BranchName) : IIntegrationEvent;
}
