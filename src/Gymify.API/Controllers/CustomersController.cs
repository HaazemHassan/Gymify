using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Gymify.API.Common.Constants;
using Gymify.API.Requests.Customers.ChangeCustomerApprovalStatus;
using Gymify.API.Requests.Customers.GetCustomers;
using Gymify.API.Requests.Customers.UpdateCustomer;
using Gymify.Application.Features.Customers.Commands.ChangeCustomerApprovalStatus;
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
        /// Retrieves a paginated list of customers with search and filters by name, email, phone, and approval status. Requires UsersRead permission.
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
        /// Retrieves a customer's full details by customer ID. Requires UsersRead permission.
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
        /// Changes the approval status of a customer (Approved, Rejected, Pending). Requires UsersWrite permission.
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
        /// Updates customer details and domain user properties. All fields are nullable for partial or full updates. Requires UsersWrite permission.
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
    }
}
