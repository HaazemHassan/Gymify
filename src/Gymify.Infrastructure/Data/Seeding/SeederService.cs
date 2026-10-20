using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Gymify.Application.ServicesContracts.Infrastructure;
using Gymify.Domain.Common.Enums;
using Gymify.Domain.Entities;
using Gymify.Domain.RepositoriesContracts;
using Gymify.Infrastructure.Data.Identity.Entities;

namespace Gymify.Infrastructure.Data.Seeding
{
    internal class SeederService : ISeederService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly AppDbContext _context;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IApplicationUserService _applicationUserService;
        private readonly IPhoneNumberService _phoneNumberService;
        private readonly ILogger<SeederService> _logger;

        public SeederService(
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager,
            IUnitOfWork unitOfWork,
            AppDbContext context,
            IApplicationUserService applicationUserService,
            IPhoneNumberService phoneNumberService,
            ILogger<SeederService> logger)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _unitOfWork = unitOfWork;
            _context = context;
            _applicationUserService = applicationUserService;
            _phoneNumberService = phoneNumberService;
            _logger = logger;
        }

        public async Task SeedRolesAsync(List<RoleSeedDto> rolesSeedData, CancellationToken cancellationToken = default)
        {
            if (rolesSeedData is null || rolesSeedData.Count == 0)
                return;

            await using var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);

            foreach (var roleData in rolesSeedData)
            {
                if (string.IsNullOrWhiteSpace(roleData.Name))
                    continue;

                var role = await _roleManager.FindByNameAsync(roleData.Name);

                if (role is null)
                {
                    role = new ApplicationRole(roleData.Name);

                    var createResult = await _roleManager.CreateAsync(role);

                    if (!createResult.Succeeded)
                        continue;
                }

                await SyncRolePermissionsAsync(
                    role.Id,
                    roleData.Permissions,
                    cancellationToken);
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
        }

        private async Task SyncRolePermissionsAsync(int roleId, List<string> permissionStrings, CancellationToken cancellationToken)
        {
            var validPermissions = new HashSet<Permission>();

            foreach (var permissionString in permissionStrings)
            {
                if (Enum.TryParse<Permission>(permissionString, ignoreCase: true, out var parsedPermission))
                {
                    validPermissions.Add(parsedPermission);
                }
            }

            var existingRolePermissions = await _context.Set<RolePermission>()
                .Where(rp => rp.RoleId == roleId)
                .ToListAsync(cancellationToken);

            var existingPermissionValues = existingRolePermissions
                .Select(rp => rp.Permission)
                .ToHashSet();

            var permissionsToAdd = validPermissions
                .Where(p => !existingPermissionValues.Contains(p))
                .Select(p => new RolePermission
                {
                    RoleId = roleId,
                    Permission = p
                })
                .ToList();

            var permissionsToRemove = existingRolePermissions
                .Where(rp => !validPermissions.Contains(rp.Permission))
                .ToList();

            if (permissionsToAdd.Count > 0)
                await _context.Set<RolePermission>().AddRangeAsync(permissionsToAdd, cancellationToken);

            if (permissionsToRemove.Count > 0)
                _context.Set<RolePermission>().RemoveRange(permissionsToRemove);
        }

        public async Task SeedUsersAsync(List<UserSeedDto> usersSeedData, CancellationToken cancellationToken = default)
        {
            if (usersSeedData is null || usersSeedData.Count == 0)
                return;

            await using var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);

            try
            {
                foreach (var userData in usersSeedData)
                {
                    var existingUser = await _userManager.FindByEmailAsync(userData.Email);
                    if (existingUser is not null)
                        continue;

                    var normalizedPhone = _phoneNumberService.Normalize(userData.PhoneNumber);

                    var userType = userData.UserType != UserType.Unknown
                        ? userData.UserType
                        : userData.Role switch
                        {
                            UserRole.SuperAdmin => UserType.SuperAdmin,
                            UserRole.Customer => UserType.Customer,
                            UserRole.Trainer or UserRole.Manager or UserRole.Gator => UserType.Employee,
                            _ => UserType.Customer
                        };

                    var domainUser = new DomainUser(
                        userType,
                        userData.Name,
                        userData.Email,
                        normalizedPhone,
                        userData.IsActive
                    );

                    var result = await _applicationUserService.AddUser(domainUser, userData.Password, userData.Role, cancellationToken);

                    if (result.IsError)
                    {
                        throw new Exception($"Failed to create user {userData.Email}: {result.FirstError.Description}");
                    }

                    if (userType == UserType.Customer || userData.Role == UserRole.Customer)
                    {
                        var customer = new Customer(
                            domainUser.Id,
                            userData.Height,
                            userData.Weight,
                            userData.ApprovalStatus ?? ApprovalStatus.Approved);

                        await _unitOfWork.Customers.AddAsync(customer, cancellationToken);
                        await _unitOfWork.SaveChangesAsync(cancellationToken);
                    }
                    else if (userType == UserType.Employee || userData.Role is UserRole.Trainer or UserRole.Manager or UserRole.Gator)
                    {
                        var employee = new Employee(
                            domainUserId: domainUser.Id,
                            salary: userData.Salary ?? 0m,
                            isActive: true);

                        await _unitOfWork.Employees.AddAsync(employee, cancellationToken);
                        await _unitOfWork.SaveChangesAsync(cancellationToken);

                        if (userData.Role == UserRole.Trainer)
                        {
                            var trainer = new Trainer(
                                employeeId: employee.Id,
                                yearsOfExperience: userData.YearsOfExperience ?? 0,
                                isActive: true);

                            await _unitOfWork.Trainers.AddAsync(trainer, cancellationToken);
                            await _unitOfWork.SaveChangesAsync(cancellationToken);
                        }
                    }

                    var createdUser = await _userManager.FindByEmailAsync(userData.Email);

                    if (createdUser is not null)
                    {
                        createdUser.EmailConfirmed = true;
                        await _userManager.UpdateAsync(createdUser);

                        if (userData.Role is not null)
                            await _userManager.AddToRoleAsync(createdUser, userData.Role.ToString()!);
                    }
                }

                await transaction.CommitAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to seed users: {Message}", ex.Message);
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
        }
    }
}
