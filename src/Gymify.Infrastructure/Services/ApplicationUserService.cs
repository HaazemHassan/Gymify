using AutoMapper;
using ErrorOr;
using Gymify.Application.Features.Users.Common;
using Gymify.Application.ServicesContracts.Infrastructure;
using Gymify.Domain.Common.Constants;
using Gymify.Domain.Common.Enums;
using Gymify.Domain.Entities;
using Gymify.Domain.RepositoriesContracts;
using Gymify.Infrastructure.Data.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Gymify.Infrastructure.Services
{
    internal class ApplicationUserService : IApplicationUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        public ApplicationUserService(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<ErrorOr<int>> AddUser(
             DomainUser domainUser,
             string password,
             bool emailConfirmed = false,
             CancellationToken ct = default)
        {
            var isAuthUserLinked = await _userManager.Users.AnyAsync(au => au.DomainUserId == domainUser.Id || au.Email == domainUser.Email, ct);

            if (isAuthUserLinked)
            {
                return Error.Conflict(
                    code: ErrorCodes.User.EmailAlreadyExists,
                    description: "Email already exists");
            }


            if (!string.IsNullOrWhiteSpace(domainUser.PhoneNumber))
            {
                var phoneExists = await _userManager.Users.AnyAsync(au => au.PhoneNumber == domainUser.PhoneNumber, ct);
                if (phoneExists)
                {
                    return Error.Conflict(
                        code: ErrorCodes.User.PhoneAlreadyExists,
                        description: "Phone number already exists");
                }
            }

            await _unitOfWork.Users.AddAsync(domainUser);


            var applicationUser = new ApplicationUser(domainUser.Email, domainUser.PhoneNumber);
            applicationUser.LinkToDomainUser(domainUser);
            if (emailConfirmed)
            {
                applicationUser.EmailConfirmed = true;
            }

            var result = await _userManager.CreateAsync(applicationUser, password);

            if (!result.Succeeded)
            {
                return Error.Failure(description: "Failed to create user");
            }

            return applicationUser.Id;
        }

        public async Task<ErrorOr<Success>> AssignRoleAsync(int userId, UserRole role, CancellationToken ct = default)
        {
            var appUser = await _userManager.Users
                .FirstOrDefaultAsync(u => u.Id == userId || u.DomainUserId == userId, ct);

            if (appUser is null)
            {
                return Error.NotFound(code: ErrorCodes.User.UserNotFound, description: "User not found.");
            }

            var roleName = role.ToString();
            if (!await _userManager.IsInRoleAsync(appUser, roleName))
            {
                var result = await _userManager.AddToRoleAsync(appUser, roleName);
                if (!result.Succeeded)
                {
                    return Error.Failure(description: "Failed to assign role to user.");
                }
            }

            return Result.Success;
        }

        public async Task<ErrorOr<UserBaseResponse>> GetUserAuthProfileAsync(int domainUserId, CancellationToken ct = default)
        {
            var appUser = await _userManager.Users
                .Include(au => au.DomainUser!)
                .FirstOrDefaultAsync(u => u.DomainUserId == domainUserId, cancellationToken: ct);

            if (appUser is null || appUser.DomainUser is null)
            {
                return Error.NotFound(code: ErrorCodes.User.UserNotFound, description: "User not found.");
            }

            return await MapToUserBaseResponseAsync(appUser);
        }

        public async Task<bool> IsEmailConfirmedAsync(int domainUserId, CancellationToken ct = default)
        {
            var appUser = await _userManager.Users.FirstOrDefaultAsync(u => u.DomainUserId == domainUserId, ct);
            return appUser is not null && appUser.EmailConfirmed;
        }

        public async Task<ErrorOr<Success>> UpdateUnconfirmedUserAsync(DomainUser domainUser, string newPassword, CancellationToken ct = default)
        {
            var appUser = await _userManager.Users.FirstOrDefaultAsync(u => u.DomainUserId == domainUser.Id, ct);
            if (appUser is null)
            {
                return Error.NotFound(code: ErrorCodes.User.UserNotFound, description: "User not found.");
            }

            if (appUser.EmailConfirmed)
            {
                return Error.Conflict(code: ErrorCodes.User.EmailAlreadyExists, description: "Email already exists and is confirmed.");
            }

            appUser.PhoneNumber = domainUser.PhoneNumber;
            var resetToken = await _userManager.GeneratePasswordResetTokenAsync(appUser);
            var resetPassResult = await _userManager.ResetPasswordAsync(appUser, resetToken, newPassword);
            if (!resetPassResult.Succeeded)
            {
                return Error.Failure(description: string.Join(", ", resetPassResult.Errors.Select(e => e.Description)));
            }

            await _userManager.UpdateAsync(appUser);
            return Result.Success;
        }

        #region HELPERS

        private async Task<UserBaseResponse> MapToUserBaseResponseAsync(ApplicationUser appUser)
        {
            var response = _mapper.Map<UserBaseResponse>(appUser.DomainUser);
            response.EmailConfirmed = appUser.EmailConfirmed;


            var roles = await _userManager.GetRolesAsync(appUser);
            var userRoleStr = roles.FirstOrDefault();
            if (userRoleStr is not null && Enum.TryParse<UserRole>(userRoleStr, out var userRole))
            {
                response.UserRole = userRole;
            }

            return response;

        }
        #endregion
    }
}
