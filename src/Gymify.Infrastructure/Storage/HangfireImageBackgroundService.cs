using Hangfire;
using Gymify.Application.ServicesContracts.Infrastructure;
using Gymify.Infrastructure.Storage.Jobs;

namespace Gymify.Infrastructure.Storage
{
    public class HangfireImageBackgroundService : IImageBackgroundService
    {
        private readonly IBackgroundJobClient _backgroundJobClient;

        public HangfireImageBackgroundService(IBackgroundJobClient backgroundJobClient)
        {
            _backgroundJobClient = backgroundJobClient;
        }

        public void DeleteImage(string publicId)
        {
            _backgroundJobClient.Enqueue<ImageCleanupJob>(job => job.Process(publicId));
        }
    }
}

