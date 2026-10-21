namespace Gymify.Application.Features.Lockers.Queries.GetLockers
{
    public class GetLockersQueryResponse
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public bool IsActive { get; set; }
        public bool IsAvailable { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }

        public GetLockersQueryResponse()
        {
        }

        public GetLockersQueryResponse(
            int id,
            int number,
            bool isActive,
            bool isAvailable,
            DateTimeOffset createdAt = default,
            DateTimeOffset? updatedAt = null)
        {
            Id = id;
            Number = number;
            IsActive = isActive;
            IsAvailable = isAvailable;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
        }
    }
}
