using AutoMapper;
using Gymify.Api.Requests.Users.CheckEmailAvailability;
using Gymify.Api.Requests.Users.UpdateProfile;
using Gymify.API.Common.Constants;
using Gymify.Application.Contracts.Api;
using Gymify.Application.Features.Users.Commands.UpdateProfile;
using Gymify.Application.Features.Users.Queries.CheckEmailAvailability;
using Gymify.Application.Features.Users.Queries.GetUserById;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gymify.API.Controllers
{
    public class UsersController(ICurrentUserService _currentUserService, IMapper _mapper) : BaseController
    {
        [HttpGet("{Id:int}", Name = RouteNames.Users.GetUserById)]
        public async Task<IActionResult> GetById([FromRoute] int Id)
        {
            var query = new GetUserByIdQuery(Id);
            var result = await Mediator.Send(query);
            if (result.IsError)
                return Problem(result.Errors);
            return Ok(result.Value);
        }

        [HttpGet("check-email-availability")]
        public async Task<IActionResult> CheckEmailAvailability([FromQuery] CheckEmailAvailabilityRequest request)
        {
            var query = _mapper.Map<CheckEmailAvailabilityQuery>(request);
            var result = await Mediator.Send(query);
            if (result.IsError)
                return Problem(result.Errors);
            return Ok(result.Value);
        }

        //[HttpGet("me")]
        //[Authorize]
        //public async Task<IActionResult> GetMe()
        //{
        //    var result = await Mediator.Send(new GetMeQuery());
        //    if (result.IsError)
        //    {
        //        return Problem(result.Errors);
        //    }

        //    return Ok(result.Value);
        //}

        [HttpPatch("me")]
        [Authorize]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileRequest request)
        {
            var command = _mapper.Map<UpdateProfileCommand>(request);
            command.UserId = _currentUserService.UserId!.Value;

            var result = await Mediator.Send(command);

            if (result.IsError)
            {
                return Problem(result.Errors);
            }

            return Ok(result.Value);
        }
    }
}
