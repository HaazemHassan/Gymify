namespace Gymify.Application.Features.Packages.Commands.CreatePackage
{
    public class CreatePackageCommandResponse
    {
        public int Id { get; set; }

        public CreatePackageCommandResponse()
        {
        }

        public CreatePackageCommandResponse(int id)
        {
            Id = id;
        }
    }
}
