using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Gymify.API.Common.Constants;
using Gymify.API.Requests.Customers.ChangeCustomerApprovalStatus;
using Gymify.API.Requests.Customers.CreateCustomer;
using Gymify.API.Requests.Customers.CreateCustomerBehaviorNote;
using Gymify.API.Requests.Customers.GetCustomerAttendances;
using Gymify.API.Requests.Customers.GetCustomerBehaviorNotes;
using Gymify.API.Requests.Customers.GetCustomers;
using Gymify.API.Requests.Customers.UpdateCustomer;
using Gymify.Application.Common.Pagination;
using Gymify.Application.Features.Attendances.Queries.GetCustomerAttendances;
using Gymify.Application.Features.CustomerBehaviorNotes.Commands.CreateCustomerBehaviorNote;
using Gymify.Application.Features.CustomerBehaviorNotes.Queries.GetCustomerBehaviorNotes;
using Gymify.Application.Features.Customers.Commands.ChangeCustomerApprovalStatus;
using Gymify.Application.Features.Customers.Commands.CreateCustomer;
using Gymify.Application.Features.Customers.Commands.UpdateCustomer;
using Gymify.Application.Features.Customers.Queries.GetCustomerById;
using Gymify.Application.Features.Customers.Queries.GetCustomerMe;
using Gymify.Application.Features.Customers.Queries.GetCustomers;

namespace Gymify.API.Controllers
{
    [Route("api/customers")]
    public class CustomersController : BaseController
    {
        private readonly IMapper _mapper;

        public CustomersController(IMapper mapper)
        {
            _mapper = mapper;
        }

        /// <summary>
        /// Creates a new customer with user account and health profile. Requires CustomersWrite permission.
        /// </summary>
        [HttpPost]
        [Authorize]
        [ProducesResponseType(typeof(CreateCustomerCommandResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> CreateCustomer([FromBody] CreateCustomerRequest request, CancellationToken ct)
        {
            var command = _mapper.Map<CreateCustomerCommand>(request);
            var result = await Mediator.Send(command, ct);

            if (result.IsError)
            {
                return Problem(result.Errors);
            }

            return CreatedAtRoute(RouteNames.Customers.GetCustomerById, new { id = result.Value.CustomerId }, result.Value);
        }

        /// <summary>
        /// Retrieves the profile of the currently authenticated customer.
        /// </summary>
        [HttpGet("me")]
        [Authorize]
        public async Task<IActionResult> GetCustomerMe()
        {
            var result = await Mediator.Send(new GetCustomerMeQuery());
            if (result.IsError)
            {
                return Problem(result.Errors);
            }

            return Ok(result.Value);
        }

        /// <summary>
        /// Retrieves a paginated list of customers with search and filters by name, email, phone, and approval status. Requires CustomersRead permission.
        /// </summary>
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetCustomers([FromQuery] GetCustomersRequest request, CancellationToken ct)
        {
            var query = _mapper.Map<GetCustomersQuery>(request);
            var result = await Mediator.Send(query, ct);

            if (result.IsError)
            {
                return Problem(result.Errors);
            }

            return Ok(result.Value);
        }

        /// <summary>
        /// Retrieves a customer's full details by customer ID. Requires CustomersRead permission.
        /// </summary>
        [HttpGet("{id:int}", Name = RouteNames.Customers.GetCustomerById)]
        [Authorize]
        [ProducesResponseType(typeof(GetCustomerByIdQueryResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCustomerById([FromRoute] int id, CancellationToken ct)
        {
            var result = await Mediator.Send(new GetCustomerByIdQuery(id), ct);

            if (result.IsError)
            {
                return Problem(result.Errors);
            }

            return Ok(result.Value);
        }

        /// <summary>
        /// Changes the approval status of a customer (Approved, Rejected, Pending). Requires CustomersWrite permission.
        /// </summary>
        [HttpPatch("{id:int}/approval-status")]
        [Authorize]
        [ProducesResponseType(typeof(ChangeCustomerApprovalStatusCommandResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> ChangeCustomerApprovalStatus(
            [FromRoute] int id,
            [FromBody] ChangeCustomerApprovalStatusRequest request,
            CancellationToken ct)
        {
            var command = _mapper.Map<ChangeCustomerApprovalStatusCommand>(request);
            command.CustomerId = id;

            var result = await Mediator.Send(command, ct);

            if (result.IsError)
            {
                return Problem(result.Errors);
            }

            return Ok(result.Value);
        }

        /// <summary>
        /// Updates customer details and domain user properties. All fields are nullable for partial or full updates. Requires CustomersWrite permission.
        /// </summary>
        [HttpPatch("{id:int}")]
        [Authorize]
        [ProducesResponseType(typeof(UpdateCustomerCommandResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> UpdateCustomer(
            [FromRoute] int id,
            [FromBody] UpdateCustomerRequest request,
            CancellationToken ct)
        {
            var command = _mapper.Map<UpdateCustomerCommand>(request);
            command.CustomerId = id;

            var result = await Mediator.Send(command, ct);

            if (result.IsError)
            {
                return Problem(result.Errors);
            }

            return Ok(result.Value);
        }

        /// <summary>
        /// Creates a behavior note for the specified customer. Allowed for CustomerBehaviorWrite or CustomersWrite permissions.
        /// </summary>
        [HttpPost("{id:int}/behavior-notes")]
        [Authorize]
        [ProducesResponseType(typeof(CreateCustomerBehaviorNoteCommandResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CreateCustomerBehaviorNote(
            [FromRoute] int id,
            [FromBody] CreateCustomerBehaviorNoteRequest request,
            CancellationToken ct)
        {
            var command = _mapper.Map<CreateCustomerBehaviorNoteCommand>(request);
            command.CustomerId = id;

            var result = await Mediator.Send(command, ct);
            if (result.IsError)
            {
                return Problem(result.Errors);
            }

            return Ok(result.Value);
        }

        /// <summary>
        /// Retrieves paginated behavior notes for the specified customer. Requires CustomersRead permission.
        /// </summary>
        [HttpGet("{id:int}/behavior-notes")]
        [Authorize]
        [ProducesResponseType(typeof(PaginatedResult<GetCustomerBehaviorNotesQueryResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCustomerBehaviorNotes(
            [FromRoute] int id,
            [FromQuery] GetCustomerBehaviorNotesRequest request,
            CancellationToken ct)
        {
            var query = _mapper.Map<GetCustomerBehaviorNotesQuery>(request);
            query.CustomerId = id;

            var result = await Mediator.Send(query, ct);
            if (result.IsError)
            {
                return Problem(result.Errors);
            }

            return Ok(result.Value);
        }

        /// <summary>
        /// Retrieves attendance history for a customer filtered by month and year. Requires AttendanceRead permission or customer self-access.
        /// </summary>
        [HttpGet("{id:int}/attendances")]
        [Authorize]
        [ProducesResponseType(typeof(GetCustomerAttendancesQueryResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCustomerAttendances(
            [FromRoute] int id,
            [FromQuery] GetCustomerAttendancesRequest request,
            CancellationToken ct)
        {
            var query = new GetCustomerAttendancesQuery(id, request.Month, request.Year);
            var result = await Mediator.Send(query, ct);

            if (result.IsError)
            {
                return Problem(result.Errors);
            }

            return Ok(result.Value);
        }
    }
}
