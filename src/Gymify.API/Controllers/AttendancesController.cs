using AutoMapper;
using Gymify.API.Requests.Attendances.CheckIn;
using Gymify.API.Requests.Attendances.CheckOut;
using Gymify.Application.Features.Attendances.Commands.CheckIn;
using Gymify.Application.Features.Attendances.Commands.CheckOut;
using Gymify.Application.Features.Attendances.Queries.GetCurrentGymOccupancy;
using Microsoft.AspNetCore.Mvc;

namespace Gymify.API.Controllers
{
    public class AttendancesController : BaseController
    {
        private readonly IMapper _mapper;

        public AttendancesController(IMapper mapper)
        {
            _mapper = mapper;
        }

        /// <summary>
        /// Checks in a customer by their unique code and assigns a locker. Requires AttendanceCheckIn permission.
        /// </summary>
        [HttpPost("check-in")]
        public async Task<IActionResult> CheckIn([FromBody] CheckInRequest request, CancellationToken ct)
        {
            var command = _mapper.Map<CheckInCommand>(request);
            var result = await Mediator.Send(command, ct);

            if (result.IsError)
            {
                return Problem(result.Errors);
            }

            return Ok(result.Value);
        }

        /// <summary>
        /// Checks out a customer by closing their currently active attendance session. Requires AttendanceCheckOut permission.
        /// </summary>
        [HttpPost("check-out")]
        public async Task<IActionResult> CheckOut([FromBody] CheckOutRequest request, CancellationToken ct)
        {
            var command = _mapper.Map<CheckOutCommand>(request);
            var result = await Mediator.Send(command, ct);

            if (result.IsError)
            {
                return Problem(result.Errors);
            }

            return Ok(result.Value);
        }

        /// <summary>
        /// Retrieves the current number of people inside the gym (active check-ins). Requires AttendanceRead permission.
        /// </summary>
        [HttpGet("current-occupancy")]
        public async Task<IActionResult> GetCurrentGymOccupancy(CancellationToken ct)
        {
            var query = new GetCurrentGymOccupancyQuery();
            var result = await Mediator.Send(query, ct);

            if (result.IsError)
            {
                return Problem(result.Errors);
            }

            return Ok(result.Value);
        }
    }
}
