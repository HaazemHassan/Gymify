namespace Gymify.Application.Security.Markers
{

    public interface IOwnedResourceRequest : IAuthorizedRequest
    {
        public int UserId { get; }
    }
}
