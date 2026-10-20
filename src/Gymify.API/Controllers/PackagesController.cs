using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Gymify.API.Common.Constants;
using Gymify.Api.Requests.Common.Pagination;
using Gymify.Api.Requests.Packages.CreatePackage;
using Gymify.Api.Requests.Packages.UpdatePackage;
using Gymify.Application.Features.Packages.Commands.CreatePackage;
using Gymify.Application.Features.Packages.Commands.UpdatePackage;
using Gymify.Application.Features.Packages.Queries.GetPackageById;
using Gymify.Application.Features.Packages.Queries.GetPackageByIdForManagement;
using Gymify.Application.Features.Packages.Queries.GetPackagesForCustomers;
using Gymify.Application.Features.Packages.Queries.GetPackagesForManagement;

namespace Gymify.API.Controllers
{
    public class PackagesController : BaseController
    {
        private readonly IMapper _mapper;

        public PackagesController(IMapper mapper)
        {
            _mapper = mapper;
        }

        /// <summary>
        /// Retrieves all active packages for customers / public view with pagination.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetPackagesForCustomers([FromQuery] BasicPaginationRequest request, CancellationToken ct)
        {
            var query = _mapper.Map<GetPackagesForCustomersQuery>(request);
            var result = await Mediator.Send(query, ct);

            if (result.IsError)
            {
                return Problem(result.Errors);
            }

            return Ok(result.Value);
        }

        /// <summary>
        /// Retrieves all packages (active & inactive) for management with pagination. Requires PackagesRead permission.
        /// </summary>
        [HttpGet("management")]
        [Authorize]
        public async Task<IActionResult> GetPackagesForManagement([FromQuery] BasicPaginationRequest request, CancellationToken ct)
        {
            var query = _mapper.Map<GetPackagesForManagementQuery>(request);
            var result = await Mediator.Send(query, ct);

            if (result.IsError)
            {
                return Problem(result.Errors);
            }

            return Ok(result.Value);
        }

        /// <summary>
        /// Retrieves active package details by ID for customers. Returns 404 if package is inactive or not found.
        /// </summary>
        [HttpGet("{id:int}", Name = RouteNames.Packages.GetPackageById)]
        public async Task<IActionResult> GetPackageById([FromRoute] int id, CancellationToken ct)
        {
            var query = new GetPackageByIdQuery(id);
            var result = await Mediator.Send(query, ct);

            if (result.IsError)
            {
                return Problem(result.Errors);
            }

            return Ok(result.Value);
        }

        /// <summary>
        /// Retrieves package details by ID for management (including inactive). Requires PackagesRead permission.
        /// </summary>
        [HttpGet("management/{id:int}", Name = RouteNames.Packages.GetPackageByIdForManagement)]
        [Authorize]
        public async Task<IActionResult> GetPackageByIdForManagement([FromRoute] int id, CancellationToken ct)
        {
            var query = new GetPackageByIdForManagementQuery(id);
            var result = await Mediator.Send(query, ct);

            if (result.IsError)
            {
                return Problem(result.Errors);
            }

            return Ok(result.Value);
        }

        /// <summary>
        /// Creates a new package. Requires PackagesWrite permission.
        /// </summary>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreatePackage([FromBody] CreatePackageRequest request, CancellationToken ct)
        {
            var command = _mapper.Map<CreatePackageCommand>(request);
            var result = await Mediator.Send(command, ct);

            if (result.IsError)
            {
                return Problem(result.Errors);
            }

            return CreatedAtRoute(
                RouteNames.Packages.GetPackageByIdForManagement,
                new { id = result.Value.Id },
                result.Value);
        }

        /// <summary>
        /// Updates an existing package. Requires PackagesWrite permission.
        /// </summary>
        [HttpPut("{id:int}")]
        [Authorize]
        public async Task<IActionResult> UpdatePackage([FromRoute] int id, [FromBody] UpdatePackageRequest request, CancellationToken ct)
        {
            var command = _mapper.Map<UpdatePackageCommand>(request);
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
