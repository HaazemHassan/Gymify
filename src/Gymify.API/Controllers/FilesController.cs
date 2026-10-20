//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using Gymify.Application.Features.Files.Commands.DeleteImage;
//using Gymify.Application.Features.Files.Commands.UploadImage;

//namespace Gymify.API.Controllers
//{
//    public class FilesController : BaseController
//    {
//        [HttpPost("images")]
//        [Authorize]
//        [RequestSizeLimit(10 * 1024 * 1024)] // 10MB limit
//        public async Task<IActionResult> UploadImage([FromForm] IFormFile file, CancellationToken ct)
//        {
//            if (file is null || file.Length == 0)
//            {
//                return BadRequest("File cannot be empty.");
//            }

//            await using var stream = file.OpenReadStream();
//            var command = new UploadImageCommand
//            {
//                FileStream = stream,
//                FileName = file.FileName
//            };

//            var result = await Mediator.Send(command, ct);

//            if (result.IsError)
//            {
//                return Problem(result.Errors);
//            }

//            return Ok(result.Value);
//        }

//        [HttpDelete("images/{publicId}")]
//        [Authorize]
//        public async Task<IActionResult> DeleteImage([FromRoute] string publicId, CancellationToken ct)
//        {
//            if (string.IsNullOrWhiteSpace(publicId))
//            {
//                return BadRequest("PublicId cannot be empty.");
//            }

//            var command = new DeleteImageCommand
//            {
//                PublicId = publicId
//            };

//            var result = await Mediator.Send(command, ct);

//            if (result.IsError)
//            {
//                return Problem(result.Errors);
//            }

//            return NoContent();
//        }
//    }
//}
