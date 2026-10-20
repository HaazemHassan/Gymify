namespace Gymify.Application.Features.Lockers.Queries.GetLockers
{
    public class GetLockersQueryResponse
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public bool IsActive { get; set; }
        public bool IsAvailable { get; set; }

        public GetLockersQueryResponse()
        {
        }

        public GetLockersQueryResponse(int id, int number, bool isActive, bool isAvailable)
        {
            Id = id;
            Number = number;
            IsActive = isActive;
            IsAvailable = isAvailable;
        }
    }
}
