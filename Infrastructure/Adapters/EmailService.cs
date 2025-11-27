using Application.Interfaces;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Infrastructure.Adapters
{
    public class EmailService : IEmailService
    {
        private readonly ILogger<EmailService> _logger;

        public EmailService(ILogger<EmailService> logger)
        {
            _logger = logger;
        }

        public Task SendEmailAsync(string to, string subject, string body)
        {
            _logger.LogInformation($"Sending email to {to} with subject {subject}. Body: {body}");
            // In a real implementation, integrate with SendGrid, AWS SES, etc.
            return Task.CompletedTask;
        }
    }
}
