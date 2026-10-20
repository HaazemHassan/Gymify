namespace Gymify.Application.Features.Lockers.Commands.UpdateLocker
{
    public class UpdateLockerCommandResponse
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public bool IsActive { get; set; }

        public UpdateLockerCommandResponse()
        {
        }

        public UpdateLockerCommandResponse(int id, int number, bool isActive)
        {
            Id = id;
            Number = number;
            IsActive = isActive;
        }
    }
}
