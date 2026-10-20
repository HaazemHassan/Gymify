using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Gymify.Api.Requests.Common.Pagination;
using Gymify.Application.Features.Trainers.Queries.GetTrainersForCustomers;

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
    }
}
