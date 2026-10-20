namespace Gymify.Application.Features.Lockers.Commands.CreateLocker
{
    public class CreateLockerCommandResponse
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public bool IsActive { get; set; }

        public CreateLockerCommandResponse()
        {
        }

        public CreateLockerCommandResponse(int id, int number, bool isActive)
        {
            Id = id;
            Number = number;
            IsActive = isActive;
        }
    }
}
