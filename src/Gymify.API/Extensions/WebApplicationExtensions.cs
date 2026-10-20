using Hangfire;
using HangfireBasicAuthenticationFilter;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Gymify.Infrastructure.BackgroundJobs;
using Gymify.Infrastructure.Data;
using Gymify.Infrastructure.Data.Seeding;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Gymify.API.Extentions
{
    public static class WebApplicationExtensions
    {

        public static async Task InitializeDatabaseAsync(this WebApplication app)
        {
            #region Initialize Database
            using (var scope = app.Services.CreateScope())
            {
                // needed to dockerize the application to make the DB created and updated automatically
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                try
                {
                    await context.Database.MigrateAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Database migration error: {ex.Message}");
                    throw;
                }


                var logger = app.Services.GetRequiredService<ILogger<Program>>();

                try
                {
                    var seederService = scope.ServiceProvider.GetRequiredService<ISeederService>();

                    string contentRoot = app.Environment.ContentRootPath;
                    string rolesPath = Path.Combine(contentRoot, "DataSeedingJson", "SystemRoles.json");
                    if (!File.Exists(rolesPath)) rolesPath = "DataSeedingJson/SystemRoles.json";

                    string usersPath = Path.Combine(contentRoot, "DataSeedingJson", "Users.json");
                    if (!File.Exists(usersPath)) usersPath = "DataSeedingJson/Users.json";

                    string rolesJson = await File.ReadAllTextAsync(rolesPath);
                    string usersJson = await File.ReadAllTextAsync(usersPath);

                    List<RoleSeedDto>? rolesSeedData = JsonSerializer.Deserialize<List<RoleSeedDto>>(rolesJson);

                    var options = new JsonSerializerOptions { Converters = { new JsonStringEnumConverter() } };
                    List<UserSeedDto>? usersSeedData = JsonSerializer.Deserialize<List<UserSeedDto>>(usersJson, options);

                    await seederService.SeedRolesAsync(rolesSeedData!);
                    await seederService.SeedUsersAsync(usersSeedData!);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred while seeding the database.");
                    throw;
                }
                #endregion
            }
        }

        public static void UseCustomHangfireDashboard(this WebApplication app)
        {
            var hangfireSettings = app.Services.GetRequiredService<IOptions<HangfireOptions>>().Value;
            app.UseHangfireDashboard(hangfireSettings.DashboardPath, new DashboardOptions
            {
                Authorization =
                [
                    new HangfireCustomBasicAuthenticationFilter
                    {
                        User = hangfireSettings.Username,
                        Pass = hangfireSettings.Password
                    }
                ]
            });
        }

        public static void RegisterRecurringJobs(this WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var jobManager = scope.ServiceProvider.GetRequiredService<IRecurringJobManager>();
                var outboxOptions = scope.ServiceProvider.GetRequiredService<IOptions<OutboxOptions>>().Value;
                jobManager.RegisterRecurringJobs(outboxOptions);
            }
        }
    }
}
