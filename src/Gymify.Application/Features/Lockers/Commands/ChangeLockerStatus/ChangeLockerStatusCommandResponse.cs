namespace Gymify.Application.Features.Lockers.Commands.ChangeLockerStatus
{
    public class ChangeLockerStatusCommandResponse
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public bool IsActive { get; set; }

        public ChangeLockerStatusCommandResponse()
        {
        }

        public ChangeLockerStatusCommandResponse(int id, int number, bool isActive)
        {
            Id = id;
            Number = number;
            IsActive = isActive;
        }
    }
}
