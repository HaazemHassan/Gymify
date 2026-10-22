namespace Gymify.Application.Security.Markers
{
    public interface ICustomerResourceRequest : IAuthorizedRequest
    {
        int CustomerId { get; }
    }
}
