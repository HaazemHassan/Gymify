using ErrorOr;

namespace Gymify.Application.Features.Files.Commands.DeleteImage
{
    public class DeleteImageCommand : ICommand<Success>
    {
        public string PublicId { get; set; } = string.Empty;
    }
}
