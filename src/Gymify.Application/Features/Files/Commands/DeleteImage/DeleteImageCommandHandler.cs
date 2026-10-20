using ErrorOr;
using Gymify.Application.ServicesContracts.Infrastructure;

namespace Gymify.Application.Features.Files.Commands.DeleteImage
{
    public class DeleteImageCommandHandler : ICommandHandler<DeleteImageCommand, Success>
    {
        private readonly IImageService _imageService;

        public DeleteImageCommandHandler(IImageService imageService)
        {
            _imageService = imageService;
        }

        public async Task<ErrorOr<Success>> Handle(DeleteImageCommand request, CancellationToken cancellationToken)
        {
            return await _imageService.DeleteAsync(request.PublicId);
        }
    }
}
