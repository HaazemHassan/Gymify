namespace Gymify.Domain.Entities.Contracts
{
    public interface IHasCreator
    {
        int? CreatedBy { get; }
    }
}
