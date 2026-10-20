using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Gymify.Application.Features.Customers.Queries.GetCustomerMe;

namespace Gymify.API.Controllers
{
    [Route("api/customers")]
    public class CustomersController : BaseController
    {
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
    }
}
