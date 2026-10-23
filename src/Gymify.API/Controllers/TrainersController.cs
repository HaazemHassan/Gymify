using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Gymify.Api.Requests.Common.Pagination;
using Gymify.Application.Common.Pagination;
using Gymify.API.Common.Constants;
using Gymify.API.Requests.Trainers.CreateTrainer;
using Gymify.API.Requests.Trainers.GetTrainerCustomers;
using Gymify.API.Requests.Trainers.GetTrainersForManagement;
using Gymify.API.Requests.Trainers.UpdateTrainer;
using Gymify.Application.Features.Trainers.Commands.CreateTrainer;
using Gymify.Application.Features.Trainers.Commands.UpdateTrainer;
using Gymify.Application.Features.Trainers.Queries.GetTrainerById;
using Gymify.Application.Features.Trainers.Queries.GetTrainerCustomers;
using Gymify.Application.Features.Trainers.Queries.GetTrainerMe;
using Gymify.Application.Features.Trainers.Queries.GetTrainersForCustomers;
using Gymify.Application.Features.Trainers.Queries.GetTrainersForManagement;
using Gymify.API.Requests.Trainers.SubmitTrainerKpiEvaluations;
using Gymify.API.Requests.Trainers.GetTrainerKpiEvaluations;
using Gymify.API.Requests.Trainers.GetTrainerDailyKpiEvaluations;
using Gymify.API.Requests.Trainers.UpdateTrainerKpiEvaluations;
using Gymify.Application.Features.TrainerKpis.Commands.SubmitTrainerKpiEvaluations;
using Gymify.Application.Features.TrainerKpis.Commands.UpdateTrainerKpiEvaluations;
using Gymify.Application.Features.TrainerKpis.Queries.GetTrainerKpiEvaluations;
using Gymify.Application.Features.TrainerKpis.Queries.GetTrainerDailyKpiEvaluations;

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
        /// Retrieves customers linked to the specified trainer through active private subscriptions. Requires TrainersRead permission or SelfTrainerOnly policy.
        /// </summary>
        [HttpGet("{id:int}/customers")]
        [Authorize]
        [ProducesResponseType(typeof(PaginatedResult<GetTrainerCustomersQueryResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetTrainerCustomers(
            [FromRoute] int id,
            [FromQuery] GetTrainerCustomersRequest request,
            CancellationToken ct)
        {
            var query = _mapper.Map<GetTrainerCustomersQuery>(request);
            query.TrainerId = id;

            var result = await Mediator.Send(query, ct);

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

        /// <summary>
        /// Creates a new trainer with user account, employee profile, and trainer role. Requires TrainersWrite permission.
        /// </summary>
        [HttpPost]
        [Authorize]
        [ProducesResponseType(typeof(CreateTrainerCommandResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> CreateTrainer([FromBody] CreateTrainerRequest request, CancellationToken ct)
        {
            var command = _mapper.Map<CreateTrainerCommand>(request);
            var result = await Mediator.Send(command, ct);

            if (result.IsError)
            {
                return Problem(result.Errors);
            }

            return CreatedAtRoute(RouteNames.Trainers.GetTrainerById, new { id = result.Value.TrainerId }, result.Value);
        }

        /// <summary>
        /// Submits daily KPI evaluations for a trainer. Only one submission is permitted per trainer per day. Requires TrainersWrite permission.
        /// </summary>
        [HttpPost("{id:int}/kpi-evaluations")]
        [Authorize]
        [ProducesResponseType(typeof(SubmitTrainerKpiEvaluationsCommandResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> SubmitTrainerKpiEvaluations(
            [FromRoute] int id,
            [FromBody] SubmitTrainerKpiEvaluationsRequest request,
            CancellationToken ct)
        {
            var command = _mapper.Map<SubmitTrainerKpiEvaluationsCommand>(request);
            command.TrainerId = id;

            var result = await Mediator.Send(command, ct);

            if (result.IsError)
            {
                return Problem(result.Errors);
            }

            return Ok(result.Value);
        }

        /// <summary>
        /// Updates daily KPI evaluations for a trainer on a specific date. Requires TrainersWrite permission.
        /// </summary>
        [HttpPut("{id:int}/kpi-evaluations")]
        [Authorize]
        [ProducesResponseType(typeof(UpdateTrainerKpiEvaluationsCommandResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateTrainerKpiEvaluations(
            [FromRoute] int id,
            [FromBody] UpdateTrainerKpiEvaluationsRequest request,
            CancellationToken ct)
        {
            var command = _mapper.Map<UpdateTrainerKpiEvaluationsCommand>(request);
            command.TrainerId = id;

            var result = await Mediator.Send(command, ct);

            if (result.IsError)
            {
                return Problem(result.Errors);
            }

            return Ok(result.Value);
        }

        /// <summary>
        /// Retrieves aggregated KPI evaluations and overall average for a trainer within an optional date range. Requires TrainersRead permission.
        /// </summary>
        [HttpGet("{id:int}/kpi-evaluations", Name = RouteNames.Trainers.GetTrainerKpiEvaluations)]
        [Authorize]
        [ProducesResponseType(typeof(GetTrainerKpiEvaluationsQueryResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetTrainerKpiEvaluations(
            [FromRoute] int id,
            [FromQuery] GetTrainerKpiEvaluationsRequest request,
            CancellationToken ct)
        {
            var query = _mapper.Map<GetTrainerKpiEvaluationsQuery>(request);
            query.TrainerId = id;

            var result = await Mediator.Send(query, ct);

            if (result.IsError)
            {
                return Problem(result.Errors);
            }

            return Ok(result.Value);
        }

        /// <summary>
        /// Retrieves daily KPI evaluations for a trainer with optional date or date range filter, paginated. Requires TrainersRead permission.
        /// </summary>
        [HttpGet("{id:int}/kpi-evaluations/daily", Name = RouteNames.Trainers.GetTrainerDailyKpiEvaluations)]
        [Authorize]
        [ProducesResponseType(typeof(PaginatedResult<TrainerDailyEvaluationResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetTrainerDailyKpiEvaluations(
            [FromRoute] int id,
            [FromQuery] GetTrainerDailyKpiEvaluationsRequest request,
            CancellationToken ct)
        {
            var query = _mapper.Map<GetTrainerDailyKpiEvaluationsQuery>(request);
            query.TrainerId = id;

            var result = await Mediator.Send(query, ct);

            if (result.IsError)
            {
                return Problem(result.Errors);
            }

            return Ok(result.Value);
        }
    }
}
