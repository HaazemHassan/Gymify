using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Gymify.API.Requests.Lockers.ChangeLockerStatus;
using Gymify.API.Requests.Lockers.CreateLocker;
using Gymify.API.Requests.Lockers.UpdateLocker;
using Gymify.Application.Features.Lockers.Commands.ChangeLockerStatus;
using Gymify.Application.Features.Lockers.Commands.CreateLocker;
using Gymify.Application.Features.Lockers.Commands.UpdateLocker;
using Gymify.Application.Features.Lockers.Queries.GetLockers;

namespace Gymify.API.Controllers
{
    [Authorize]
    public class LockersController : BaseController
    {
        private readonly IMapper _mapper;

        public LockersController(IMapper mapper)
        {
            _mapper = mapper;
        }

        /// <summary>
        /// Retrieves all lockers with optional isActive filter and real-time availability status. Requires LockersRead permission.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetLockers([FromQuery] bool? isActive, CancellationToken ct)
        {
            var query = new GetLockersQuery(isActive);
            var result = await Mediator.Send(query, ct);

            if (result.IsError)
            {
                return Problem(result.Errors);
            }

            return Ok(result.Value);
        }

        /// <summary>
        /// Creates a new locker with the specified number. Requires LockersWrite permission.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateLocker([FromBody] CreateLockerRequest request, CancellationToken ct)
        {
            var command = _mapper.Map<CreateLockerCommand>(request);
            var result = await Mediator.Send(command, ct);

            if (result.IsError)
            {
                return Problem(result.Errors);
            }

            return Ok(result.Value);
        }

        /// <summary>
        /// Changes the active/inactive status of a locker. Requires LockersWrite permission.
        /// </summary>
        [HttpPatch("{id:int}/status")]
        public async Task<IActionResult> ChangeLockerStatus(
            [FromRoute] int id,
            [FromBody] ChangeLockerStatusRequest request,
            CancellationToken ct)
        {
            var command = _mapper.Map<ChangeLockerStatusCommand>(request);
            command.Id = id;

            var result = await Mediator.Send(command, ct);

            if (result.IsError)
            {
                return Problem(result.Errors);
            }

            return Ok(result.Value);
        }

        /// <summary>
        /// Updates an existing locker (number and/or status). Requires LockersWrite permission.
        /// </summary>
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateLocker(
            [FromRoute] int id,
            [FromBody] UpdateLockerRequest request,
            CancellationToken ct)
        {
            var command = _mapper.Map<UpdateLockerCommand>(request);
            command.Id = id;

            var result = await Mediator.Send(command, ct);

            if (result.IsError)
            {
                return Problem(result.Errors);
            }

            return Ok(result.Value);
        }
    }
}
