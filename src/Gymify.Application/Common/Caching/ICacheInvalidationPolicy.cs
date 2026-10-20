namespace Gymify.Application.Common.Caching
{
    public interface ICacheInvalidationPolicy<in TRequest> where TRequest : IBaseCommand
    {
        IEnumerable<string> GetTags(TRequest request);
    }
}
