using Hangfire;
using Gymify.Infrastructure.Messaging.Jobs;
using Gymify.Infrastructure.Security.Jobs;

namespace Gymify.Infrastructure.BackgroundJobs
{
    public static class HangfireExtensions
    {
        public static void RegisterRecurringJobs(this IRecurringJobManager manager, OutboxOptions outboxOptions)
        {
            manager.AddOrUpdate<RefreshTokensCleanupJob>(
                RefreshTokensCleanupJob.JobId,
                job => job.ExecuteAsync(),
                RefreshTokensCleanupJob.Schedule
            );

            string outboxSchedule = outboxOptions.ProcessingIntervalSeconds < 60
                ? $"*/{outboxOptions.ProcessingIntervalSeconds} * * * * *"
                : $"*/{outboxOptions.ProcessingIntervalSeconds / 60} * * * *";

            manager.AddOrUpdate<ProcessOutboxMessagesJob>(
                ProcessOutboxMessagesJob.JobId,
                job => job.ExecuteAsync(CancellationToken.None),
                outboxSchedule
            );
        }
    }
}
