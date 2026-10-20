using ErrorOr;
using Gymify.Application.Common.DTOS;
using Gymify.Application.ServicesContracts.Infrastructure;

namespace Gymify.Application.Features.Files.Commands.UploadImage
{
    public class UploadImageCommandHandler : ICommandHandler<UploadImageCommand, ImageUploadDTO>
    {
        private readonly IImageService _imageService;

        public UploadImageCommandHandler(IImageService imageService)
        {
            _imageService = imageService;
        }

        public async Task<ErrorOr<ImageUploadDTO>> Handle(UploadImageCommand request, CancellationToken cancellationToken)
        {
            return await _imageService.UploadAsync(request.FileStream, request.FileName, cancellationToken);
        }
    }
}
