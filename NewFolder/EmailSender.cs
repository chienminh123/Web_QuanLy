using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Web.NewFolder
{
    public class EmailSender : IEmailSender
        {
            public AuthMessageSenderOptions Options { get; set; }

            public EmailSender(IOptions<AuthMessageSenderOptions> optionsAccessor)
            {
                Options = optionsAccessor.Value;
            }

            public async Task SendEmailAsync(string email, string subject, string message)
            {
                var client = new SendGridClient(Options.SendGridKey);
                var from = new EmailAddress(Options.SendGridUser, "Your App Name");
                var to = new EmailAddress(email);
                var msg = MailHelper.CreateSingleEmail(from, to, subject, message, message);
                await client.SendEmailAsync(msg);
            }
        }
}
