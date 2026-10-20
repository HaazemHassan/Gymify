using ErrorOr;
using Gymify.Application.Common.DTOS;

namespace Gymify.Application.Features.Files.Commands.UploadImage
{
    public class UploadImageCommand : ICommand<ImageUploadDTO>
    {
        public Stream FileStream { get; set; } = null!;
        public string FileName { get; set; } = string.Empty;
    }
}
