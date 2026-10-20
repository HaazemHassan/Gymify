namespace Gymify.Application.ServicesContracts.Infrastructure
{
    public interface IEmailService
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage);

    }
}
