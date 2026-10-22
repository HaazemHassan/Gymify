namespace Gymify.Application.Security.Markers
{
    public interface ITrainerResourceRequest : IAuthorizedRequest
    {
        int TrainerId { get; }
    }
}
