using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Gymify.Api.Requests.Common.Pagination;
using Gymify.Application.Common.Pagination;
using Gymify.API.Common.Constants;
using Gymify.API.Requests.Trainers.GetTrainersForManagement;
using Gymify.API.Requests.Trainers.UpdateTrainer;
using Gymify.Application.Features.Trainers.Commands.UpdateTrainer;
using Gymify.Application.Features.Trainers.Queries.GetTrainerById;
using Gymify.Application.Features.Trainers.Queries.GetTrainerMe;
using Gymify.Application.Features.Trainers.Queries.GetTrainersForCustomers;
using Gymify.Application.Features.Trainers.Queries.GetTrainersForManagement;

namespace Gymify.API.Controllers
{
    public class TrainersController : BaseController
    {
        private readonly IMapper _mapper;

        public TrainersController(IMapper mapper)
        {
            _mapper = mapper;
        }

        /// <summary>
        /// Retrieves the profile of the currently authenticated trainer.
        /// </summary>
        [HttpGet("me")]
        [Authorize]
        [ProducesResponseType(typeof(GetTrainerMeQueryResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetTrainerMe()
        {
            var result = await Mediator.Send(new GetTrainerMeQuery());
            if (result.IsError)
            {
                return Problem(result.Errors);
            }

            return Ok(result.Value);
        }

        /// <summary>
        /// Retrieves active gym trainers for customers / public view with pagination and search.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetTrainersForCustomers([FromQuery] BasicPaginationRequest request, CancellationToken ct)
        {
            var query = _mapper.Map<GetTrainersForCustomersQuery>(request);
            var result = await Mediator.Send(query, ct);

            if (result.IsError)
            {
                return Problem(result.Errors);
            }

            return Ok(result.Value);
        }

        /// <summary>
        /// Retrieves all trainers for management with pagination, search, and active status filter. Requires TrainersRead permission.
        /// </summary>
        [HttpGet("management")]
        [Authorize]
        [ProducesResponseType(typeof(PaginatedResult<GetTrainersForManagementQueryResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetTrainersForManagement([FromQuery] GetTrainersForManagementRequest request, CancellationToken ct)
        {
            var query = _mapper.Map<GetTrainersForManagementQuery>(request);
            var result = await Mediator.Send(query, ct);

            if (result.IsError)
            {
                return Problem(result.Errors);
            }

            return Ok(result.Value);
        }

        /// <summary>
        /// Retrieves a trainer's full details by trainer ID. Requires TrainersRead permission.
        /// </summary>
        [HttpGet("{id:int}", Name = RouteNames.Trainers.GetTrainerById)]
        [Authorize]
        [ProducesResponseType(typeof(GetTrainerByIdQueryResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetTrainerById([FromRoute] int id, CancellationToken ct)
        {
            var result = await Mediator.Send(new GetTrainerByIdQuery(id), ct);

            if (result.IsError)
            {
                return Problem(result.Errors);
            }

            return Ok(result.Value);
        }

        /// <summary>
        /// Updates trainer details (user info, salary, experience, active status). Requires TrainersWrite permission.
        /// </summary>
        [HttpPatch("{id:int}")]
        [Authorize]
        [ProducesResponseType(typeof(UpdateTrainerCommandResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> UpdateTrainer(
            [FromRoute] int id,
            [FromBody] UpdateTrainerRequest request,
            CancellationToken ct)
        {
            var command = _mapper.Map<UpdateTrainerCommand>(request);
            command.TrainerId = id;

            var result = await Mediator.Send(command, ct);

            if (result.IsError)
            {
                return Problem(result.Errors);
            }

            return Ok(result.Value);
        }
    }
}
