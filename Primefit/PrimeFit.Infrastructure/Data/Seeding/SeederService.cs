using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PrimeFit.Application.ServicesContracts.Infrastructure;
using PrimeFit.Domain.Common.Enums;
using PrimeFit.Domain.Entities;
using PrimeFit.Domain.RepositoriesContracts;
using PrimeFit.Infrastructure.Data.Identity.Entities;
using PrimeFit.Domain.ValueObjects;
using PrimeFit.Application.Common.DTOS;
using Microsoft.Extensions.Logging;

namespace PrimeFit.Infrastructure.Data.Seeding
{
    internal class SeederService : ISeederService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly AppDbContext _context;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IApplicationUserService _applicationUserService;
        private readonly IPhoneNumberService _phoneNumberService;
        private readonly ITotpService _totpService;
        private readonly IImageService _imageService;
        private readonly ILogger<SeederService> _logger;

        public SeederService(
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager,
            IUnitOfWork unitOfWork,
            AppDbContext context,
            IApplicationUserService applicationUserService,
            IPhoneNumberService phoneNumberService,
            ITotpService totpService,
            IImageService imageService,
            ILogger<SeederService> logger)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _unitOfWork = unitOfWork;
            _context = context;
            _applicationUserService = applicationUserService;
            _phoneNumberService = phoneNumberService;
            _totpService = totpService;
            _imageService = imageService;
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

        public async Task SeedEmployeeRolesAsync(List<EmployeeRoleSeedDto> data, CancellationToken cancellationToken = default)
        {
            if (data is null || data.Count == 0)
                return;

            await using var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);

            foreach (var roleData in data)
            {
                if (string.IsNullOrWhiteSpace(roleData.Name))
                    continue;

                var employeeRole = await _context.Set<EmployeeRole>()
                    .FirstOrDefaultAsync(r => r.Name == roleData.Name, cancellationToken);

                if (employeeRole is null)
                {
                    employeeRole = new EmployeeRole(roleData.Name);
                    await _context.Set<EmployeeRole>().AddAsync(employeeRole, cancellationToken);
                    await _context.SaveChangesAsync(cancellationToken);
                }

                await SyncEmployeeRolePermissionsAsync(employeeRole.Id, roleData.Permissions, cancellationToken);
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
        }

        private async Task SyncEmployeeRolePermissionsAsync(int employeeRoleId, List<string> permissionStrings, CancellationToken cancellationToken)
        {
            var validPermissions = new HashSet<Permission>();

            foreach (var permissionString in permissionStrings)
            {
                if (Enum.TryParse<Permission>(permissionString, ignoreCase: true, out var parsedPermission))
                {
                    validPermissions.Add(parsedPermission);
                }
            }

            var existingPermissions = await _context.Set<EmployeeRolePermission>()
                .Where(rp => rp.EmployeeRoleId == employeeRoleId)
                .ToListAsync(cancellationToken);

            var existingPermissionValues = existingPermissions
                .Select(rp => rp.Permission)
                .ToHashSet();

            var permissionsToAdd = validPermissions
                .Where(p => !existingPermissionValues.Contains(p))
                .Select(p => new EmployeeRolePermission(employeeRoleId, p))
                .ToList();

            var permissionsToRemove = existingPermissions
                .Where(rp => !validPermissions.Contains(rp.Permission))
                .ToList();

            if (permissionsToAdd.Count > 0)
                await _context.Set<EmployeeRolePermission>().AddRangeAsync(permissionsToAdd, cancellationToken);

            if (permissionsToRemove.Count > 0)
                _context.Set<EmployeeRolePermission>().RemoveRange(permissionsToRemove);
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
                    var totpSecret = _totpService.GenerateTotpSecret();

                    var domainUser = new DomainUser(
                        userData.UserType,
                        userData.FirstName,
                        userData.LastName,
                        userData.Email,
                        normalizedPhone,
                        totpSecret
                    );

                    var result = await _applicationUserService.AddUser(domainUser, userData.Password, userData.Role, cancellationToken);

                    if (result.IsError)
                    {
                        throw new Exception($"Failed to create user {userData.Email}: {result.FirstError.Description}");
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
            catch
            {
                await transaction.RollbackAsync(cancellationToken);
            }
        }

        public async Task SeedEgyptBranchesAsync(CancellationToken cancellationToken = default)
        {
            if (await _context.Set<Branch>().AnyAsync(cancellationToken))
            {
                _logger.LogInformation("Branches already seeded, skipping branch seeding.");
                return;
            }

            var domainUser = await _context.Set<DomainUser>().FirstOrDefaultAsync(u => u.Email == "partneradmin@project.com", cancellationToken);
            if (domainUser is null)
            {
                _logger.LogWarning("Partner admin not found, cannot seed branches.");
                return;
            }

            string imagesDir = null;
            var currentDir = new DirectoryInfo(Directory.GetCurrentDirectory());
            while (currentDir != null)
            {
                var potentialPath = Path.Combine(currentDir.FullName, "images");
                if (Directory.Exists(potentialPath))
                {
                    imagesDir = potentialPath;
                    break;
                }
                currentDir = currentDir.Parent;
            }

            if (imagesDir == null)
            {
                throw new DirectoryNotFoundException("Could not find the 'images' directory in any parent directory.");
            }

            var uploadedImages = new List<ImageUploadDTO>();

            if (Directory.Exists(imagesDir))
            {
                var files = Directory.GetFiles(imagesDir, "*.*")
                    .Where(f => f.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) || f.EndsWith(".png", StringComparison.OrdinalIgnoreCase))
                    .ToList();
                _logger.LogInformation($"Found {files.Count} images. Uploading to Cloudinary...");

                foreach (var file in files)
                {
                    try
                    {
                        using var stream = File.OpenRead(file);
                        var fileName = Path.GetFileName(file);
                        var uploadResult = await _imageService.UploadAsync(stream, fileName, cancellationToken);

                        if (!uploadResult.IsError)
                        {
                            uploadedImages.Add(uploadResult.Value);
                        }

                        // Delay to avoid rate limiting on free tier
                        await Task.Delay(1500, cancellationToken);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, $"Error uploading image {file}");
                    }
                }
            }

            if (uploadedImages.Count == 0)
            {
                throw new InvalidOperationException("Failed to upload any branch images to Cloudinary. Seeding aborted.");
            }

            var random = new Random();
            var gymPrefixes = new[] { "Gold's", "Smart", "World", "Titanium", "Ignite", "Transformers", "H2O", "Fibers", "Body Masters", "Muscle Factory", "ProFit", "Iron Paradise", "Elite", "Fitness First", "Oxygen", "Glow", "Power", "Pulse", "Steel", "Peak", "Vigor", "Ascent", "Beast Mode", "Ironclad", "True Fit", "Force", "Endurance", "Impact", "Matrix", "Velocity", "Champion", "Alpha", "Spartan", "Titan", "Olympus", "Dynamic", "Aero", "Hyper", "Summit", "Zenith", "Prime", "Legacy", "Infinity", "NextGen", "Metro", "Flex" };
            var gymSuffixes = new[] { "Gym", "Fitness", "Club", "Center", "Studio", "Arena", "Zone", "House", "Hub", "Lab" };

            var oneMonthNames = new[] { "Basic 1 Month Membership", "Standard Monthly Pass", "Premium 1 Month Access", "Elite Monthly Membership", "VIP Unlimited Month", "Unlimited Monthly Access", "Classic Month Pass" };
            var threeMonthNames = new[] { "Standard 3 Months Membership", "Silver 3 Months Pass", "Gold 3 Months Membership", "Active 3 Months Package", "Promo 3 Months Plan", "Classic 3 Months Pass", "Vigor 3 Months Pass" };
            var sixMonthNames = new[] { "Premium 6 Months Membership", "Platinum 6 Months Pass", "Elite Half-Year Pass", "Pro 6 Months Membership", "Half-Year Fitness Plan", "Peak 6 Months Plan" };
            var twelveMonthNames = new[] { "Annual Elite Membership", "VIP Annual Pass", "Ultimate 12 Months Membership", "Yearly Platinum Package", "Full Year Fitness Membership", "Infinity Annual Pass" };

            var governorates = await _context.Set<Governorate>().ToListAsync(cancellationToken);
            if (governorates.Count == 0) return;

            var baseCoords = new Dictionary<string, (double Lat, double Lon)>
            {
                {"Cairo", (30.0444, 31.2357)}, {"Giza", (30.0131, 31.2089)}, {"Alexandria", (31.2001, 29.9187)},
                {"Dakahlia", (31.0379, 31.3815)}, {"Red Sea", (27.2579, 33.8116)}, {"Beheira", (31.0364, 30.4698)},
                {"Fayoum", (29.3084, 30.8428)}, {"Gharbia", (30.7865, 31.0004)}, {"Ismailia", (30.5965, 32.2715)},
                {"Menoufia", (30.5503, 31.0106)}, {"Minya", (28.1011, 30.7503)}, {"Qalyubia", (30.4660, 31.1793)},
                {"New Valley", (25.4390, 30.5586)}, {"Suez", (29.9668, 32.5498)}, {"Aswan", (24.0889, 32.8998)},
                {"Assiut", (27.1783, 31.1859)}, {"Beni Suef", (29.0661, 31.0994)}, {"Port Said", (31.2653, 32.3019)},
                {"Damietta", (31.4175, 31.8144)}, {"Sharkia", (30.5877, 31.5167)}, {"South Sinai", (27.9158, 34.3299)},
                {"Kafr El Sheikh", (31.1107, 30.9388)}, {"Matrouh", (31.3543, 27.2373)}, {"Luxor", (25.6872, 32.6396)},
                {"Qena", (26.1551, 32.7160)}, {"North Sinai", (31.1249, 33.7984)}, {"Sohag", (26.5591, 31.6957)}
            };

            await using var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                var branchesToAdd = new List<Branch>();
                
                foreach (var gov in governorates)
                {
                    if (!baseCoords.TryGetValue(gov.Name, out var coords)) continue;

                    for (int i = 1; i <= 7; i++)
                    {
                        var gymPrefix = gymPrefixes[random.Next(gymPrefixes.Length)];
                        var gymSuffix = gymSuffixes[random.Next(gymSuffixes.Length)];
                        var gymName = $"{gymPrefix} {gymSuffix}";
                        
                        var finalBranchName = $"{gymName} {gov.Name} {i}";
                        var branchEmail = $"{gymName.Replace(" ", "").Replace("'", "").ToLower()}{gov.Id}_{i}@example.com";
                        var branchPhone = $"01{random.Next(0, 3)}{random.Next(10000000, 99999999)}";

                        double latOffset = (random.NextDouble() * (i <= 4 ? 0.05 : 0.2)) * (random.Next(2) == 0 ? 1 : -1);
                        double lonOffset = (random.NextDouble() * (i <= 4 ? 0.05 : 0.2)) * (random.Next(2) == 0 ? 1 : -1);
                        
                        var geoLocation = GeoLocation.Create(coords.Lat + latOffset, coords.Lon + lonOffset).Value;
                        var branchType = (BranchType)random.Next(0, 3);
                        
                        var branchResult = Branch.Create(domainUser.Id, finalBranchName, branchEmail, branchPhone, branchType);
                        if (branchResult.IsError) continue;
                        
                        var branch = branchResult.Value;
                        branch.UpdateLocationDetails(gov, $"Main St., {finalBranchName} Tower, {gov.Name}", geoLocation);

                        var workingHours = new List<BranchWorkingHour>();
                        for (int day = 0; day <= 6; day++)
                        {
                            bool isClosed = (day == 5 && random.Next(5) == 0);
                            workingHours.Add(BranchWorkingHour.Create((DayOfWeek)day, new TimeOnly(6, 0), new TimeOnly(23, 0), isClosed, 0).Value);
                        }
                        branch.UpdateWorkingHours(workingHours);

                        var basePrice = random.Next(300, 800);
                        var oneMonthName = oneMonthNames[random.Next(oneMonthNames.Length)];
                        var threeMonthName = threeMonthNames[random.Next(threeMonthNames.Length)];
                        var sixMonthName = sixMonthNames[random.Next(sixMonthNames.Length)];

                        branch.AddPackage(oneMonthName, basePrice, 1, true, 1, 7);
                        branch.AddPackage(threeMonthName, (decimal)(basePrice * 2.7), 3, true, 2, 14);
                        branch.AddPackage(sixMonthName, basePrice * 5, 6, true, 3, 21);

                        // 50% chance of adding a 12-month membership
                        if (random.Next(2) == 0)
                        {
                            var twelveMonthName = twelveMonthNames[random.Next(twelveMonthNames.Length)];
                            branch.AddPackage(twelveMonthName, basePrice * 9, 12, true, 4, 30);
                        }

                        var logoUpload = uploadedImages[random.Next(uploadedImages.Count)];
                        var logo = new BranchImage(logoUpload.SecureUrl, logoUpload.PublicId, BranchImageType.Logo, 0, 1);
                        branch.AddImage(logo);
                        branch.ActivateImage(logo);

                        var mpUpload1 = uploadedImages[random.Next(uploadedImages.Count)];
                        var mp1 = new BranchImage(mpUpload1.SecureUrl, mpUpload1.PublicId, BranchImageType.MarketPlace, 0, 2);
                        branch.AddImage(mp1);
                        branch.ActivateImage(mp1);

                        var mpUpload2 = uploadedImages[random.Next(uploadedImages.Count)];
                        var mp2 = new BranchImage(mpUpload2.SecureUrl, mpUpload2.PublicId, BranchImageType.MarketPlace, 0, 3);
                        branch.AddImage(mp2);
                        branch.ActivateImage(mp2);

                        branch.Activate();
                        branchesToAdd.Add(branch);
                    }
                }

                await _context.Set<Branch>().AddRangeAsync(branchesToAdd, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);
                _logger.LogInformation("Successfully seeded 189 branches across Egypt.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to seed branches.");
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
        }
    }
}
