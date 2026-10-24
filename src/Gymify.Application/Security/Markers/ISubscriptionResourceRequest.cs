namespace Gymify.Application.Security.Markers
{
    public interface ISubscriptionResourceRequest : IAuthorizedRequest
    {
        int SubscriptionId { get; }
    }
}
