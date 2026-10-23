using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Gymify.API.Common.Constants;
using Gymify.API.Requests.Kpis.CreateKpi;
using Gymify.API.Requests.Kpis.GetKpisPaginated;
using Gymify.API.Requests.Kpis.UpdateKpi;
using Gymify.Application.Common.Pagination;
using Gymify.Application.Features.Kpis.Commands.CreateKpi;
using Gymify.Application.Features.Kpis.Commands.UpdateKpi;
using Gymify.Application.Features.Kpis.Queries.GetKpiById;
using Gymify.Application.Features.Kpis.Queries.GetKpisPaginated;

namespace Gymify.API.Controllers
{
    public class KpisController : BaseController
    {
        private readonly IMapper _mapper;

        public KpisController(IMapper mapper)
        {
            _mapper = mapper;
        }

        /// <summary>
        /// Retrieves all KPIs with pagination, search, and optional active filter. Requires TrainersRead permission.
        /// </summary>
        [HttpGet]
        [Authorize]
        [ProducesResponseType(typeof(PaginatedResult<GetKpisPaginatedQueryResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetKpis([FromQuery] GetKpisPaginatedRequest request, CancellationToken ct)
        {
            var query = _mapper.Map<GetKpisPaginatedQuery>(request);
            var result = await Mediator.Send(query, ct);

            if (result.IsError)
            {
                return Problem(result.Errors);
            }

            return Ok(result.Value);
        }

        /// <summary>
        /// Retrieves a KPI by ID. Requires TrainersRead permission.
        /// </summary>
        [HttpGet("{id:int}", Name = RouteNames.Kpis.GetKpiById)]
        [Authorize]
        [ProducesResponseType(typeof(GetKpiByIdQueryResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetKpiById([FromRoute] int id, CancellationToken ct)
        {
            var result = await Mediator.Send(new GetKpiByIdQuery(id), ct);

            if (result.IsError)
            {
                return Problem(result.Errors);
            }

            return Ok(result.Value);
        }

        /// <summary>
        /// Creates a new KPI. Requires TrainersWrite permission.
        /// </summary>
        [HttpPost]
        [Authorize]
        [ProducesResponseType(typeof(CreateKpiCommandResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> CreateKpi([FromBody] CreateKpiRequest request, CancellationToken ct)
        {
            var command = _mapper.Map<CreateKpiCommand>(request);
            var result = await Mediator.Send(command, ct);

            if (result.IsError)
            {
                return Problem(result.Errors);
            }

            return CreatedAtRoute(RouteNames.Kpis.GetKpiById, new { id = result.Value.Id }, result.Value);
        }

        /// <summary>
        /// Updates an existing KPI. Requires TrainersWrite permission.
        /// </summary>
        [HttpPatch("{id:int}")]
        [Authorize]
        [ProducesResponseType(typeof(UpdateKpiCommandResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> UpdateKpi(
            [FromRoute] int id,
            [FromBody] UpdateKpiRequest request,
            CancellationToken ct)
        {
            var command = _mapper.Map<UpdateKpiCommand>(request);
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
