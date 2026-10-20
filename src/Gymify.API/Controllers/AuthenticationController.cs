using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Gymify.Api.Requests.Authentication.ChangePassword;
using Gymify.Api.Requests.Authentication.ConfirmEmail;
using Gymify.Api.Requests.Authentication.Logout;
using Gymify.Api.Requests.Authentication.RefreshToken;
using Gymify.Api.Requests.Authentication.RegisterCustomer;
using Gymify.Api.Requests.Authentication.ResendConfirmationEmail;
using Gymify.Api.Requests.Authentication.ResetPassword;
using Gymify.Api.Requests.Authentication.SendResetPasswordEmail;
using Gymify.Api.Requests.Authentication.SignInWithGoogle;
using Gymify.Api.Requests.Authentication.SignInWithPassword;
using Gymify.Api.Requests.Authentication.ValidateResetPasswordCode;
using Gymify.API.Common.Constants;
using Gymify.API.Common.Filters;
using Gymify.Application.Contracts.Api;
using Gymify.Application.Features.Authentication.Commands.ChangePassword;
using Gymify.Application.Features.Authentication.Commands.ConfirmEmail;
using Gymify.Application.Features.Authentication.Commands.Logout;
using Gymify.Application.Features.Authentication.Commands.RefreshToken;
using Gymify.Application.Features.Authentication.Commands.RegisterCustomer;
using Gymify.Application.Features.Authentication.Commands.ResendConfirmEmail;
using Gymify.Application.Features.Authentication.Commands.ResetPassword;
using Gymify.Application.Features.Authentication.Commands.SendResetPasswordEmail;
using Gymify.Application.Features.Authentication.Commands.SignIn;
using Gymify.Application.Features.Authentication.Commands.SignInWithGoogle;
using Gymify.Application.Features.Authentication.Common;
using Gymify.Application.Features.Authentication.Queries.ValidateResetPasswordCode;

namespace Gymify.API.Controllers
{
    public class AuthenticationController(IClientContextService clientContextService, IMapper _mapper) : BaseController
    {
        private readonly IClientContextService _clientContextService = clientContextService;



        [HttpPost("register-customer")]
        [AnonymousOnly]

        public async Task<IActionResult> RegisterCustomer([FromBody] RegisterCustomerRequest request)
        {
            var command = _mapper.Map<RegisterCustomerCommand>(request);
            var result = await Mediator.Send(command);

            if (result.IsError)
                return Problem(result.Errors);

            return CreatedAtRoute(
                            routeName: RouteNames.Users.GetUserById,
                            routeValues: new { id = result.Value!.Id },
                            value: result.Value
             );


        }





        [HttpPost("login")]
        [EnableRateLimiting("loginLimiter")]
        [AnonymousOnly]

        public async Task<IActionResult> Login([FromBody] SignInWithPasswordRequest request)
        {
            var command = _mapper.Map<SignInWithPasswordCommand>(request);
            var result = await Mediator.Send(command);
            if (result.IsError)
                return Problem(result.Errors);

            HandleRefreshToken(result.Value);
            return Ok(result.Value);
        }

        [HttpPost("google")]
        [EnableRateLimiting("loginLimiter")]
        [AllowAnonymous]
        public async Task<IActionResult> SignInWithGoogle([FromBody] SignInWithGoogleRequest request)
        {
            var command = _mapper.Map<SignInWithGoogleCommand>(request);
            var result = await Mediator.Send(command);
            if (result.IsError)
            {
                return Problem(result.Errors);

            }

            HandleRefreshToken(result.Value);
            return Ok(result.Value);
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            var command = _mapper.Map<RefreshTokenCommand>(request);
            if (_clientContextService.IsWebClient())
                command.RefreshToken = Request.Cookies["refreshToken"];

            if (command.RefreshToken is null)
                return Unauthorized("Invalid Refresh token.");

            var result = await Mediator.Send(command);

            if (result.IsError)
            {
                return Problem(result.Errors);

            }

            HandleRefreshToken(result.Value);
            return Ok(result.Value);
        }

        [HttpPost("confirm-email")]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmailRequest request)
        {
            var command = _mapper.Map<ConfirmEmailCommand>(request);
            var result = await Mediator.Send(command);
            if (result.IsError)
            {
                return Problem(result.Errors);

            }

            return NoContent();
        }

        [HttpPost("validate-reset-password-code")]
        [AllowAnonymous]
        public async Task<IActionResult> ValidateResetPasswordCode([FromBody] ValidateResetPasswordCodeRequest request)
        {
            var query = _mapper.Map<ValidateResetPasswordCodeQuery>(request);
            var result = await Mediator.Send(query);
            if (result.IsError)
            {
                return Problem(result.Errors);

            }

            return Ok(result.Value);
        }



        [HttpPost("resend-confirmation-email")]
        public async Task<IActionResult> ResendConfirmationEmail([FromBody] ResendConfirmationEmailRequest request)
        {
            var command = _mapper.Map<ResendConfirmationEmailCommand>(request);
            var result = await Mediator.Send(command);
            if (result.IsError)
            {
                return Problem(result.Errors);

            }

            return NoContent();
        }

        [HttpPost("send-reset-password-email")]
        [AnonymousOnly]
        public async Task<IActionResult> SendResetPasswordEmail([FromBody] SendResetPasswordEmailRequest request)
        {
            var command = _mapper.Map<SendResetPasswordEmailCommand>(request);
            var result = await Mediator.Send(command);
            if (result.IsError)
            {
                return Problem(result.Errors);
            }

            return NoContent();
        }

        [HttpPost("reset-password")]
        [AnonymousOnly]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            var command = _mapper.Map<ResetPasswordCommand>(request);
            var result = await Mediator.Send(command);
            if (result.IsError)
            {
                return Problem(result.Errors);
            }

            return NoContent();
        }

        [HttpPost("change-password")]
        [Authorize]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            var command = _mapper.Map<ChangePasswordCommand>(request);
            var result = await Mediator.Send(command);

            if (result.IsError)
            {
                return Problem(result.Errors);
            }

            return NoContent();
        }

        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout([FromBody] LogoutRequest request)
        {
            var command = _mapper.Map<LogoutCommand>(request);
            if (_clientContextService.IsWebClient())
                command.RefreshToken = Request.Cookies["refreshToken"];

            if (command.RefreshToken is null)
                return Unauthorized("Refresh token is required");

            var result = await Mediator.Send(command);
            if (result.IsError)
                return Problem(result.Errors);

            Response.Cookies.Delete("refreshToken");
            return NoContent();
        }

        #region Helpers
        private void HandleRefreshToken(AuthResponse authResponse)
        {
            if (authResponse?.RefreshToken is null)
                return;

            var refreshToken = authResponse.RefreshToken.Token;

            if (_clientContextService.IsWebClient())
            {
                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.None,
                    Path = "/api/authentication",
                    Expires = authResponse.RefreshToken.ExpirationDate
                };
                Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
                authResponse.RefreshToken = null;
            }
        }
        #endregion
    }

}
