using MailKit.Net.Smtp;
using MimeKit;
using DotNetEnv;

namespace BookApi.Helpers
{
    public class EmailHelper
    {
        private readonly string _smtpServer;
        private readonly int _smtpPort;
        private readonly string _smtpUser;
        private readonly string _smtpPass;
        private readonly LoggingHelper _loggingHelper;

        public EmailHelper(LoggingHelper loggingHelper)
        {
            Env.Load();
            _smtpServer = Environment.GetEnvironmentVariable("SMTP_SERVER") ?? throw new ArgumentNullException("SMTP_SERVER not found in environment variables");
            _smtpPort = int.TryParse(Environment.GetEnvironmentVariable("SMTP_PORT"), out var port) ? port : throw new ArgumentNullException("SMTP_PORT not found or invalid in environment variables");
            _smtpUser = Environment.GetEnvironmentVariable("SMTP_USER") ?? throw new ArgumentNullException("SMTP_USER not found in environment variables");
            _smtpPass = Environment.GetEnvironmentVariable("SMTP_PASS") ?? throw new ArgumentNullException("SMTP_PASS not found in environment variables");
            _loggingHelper = loggingHelper ?? throw new ArgumentNullException(nameof(loggingHelper));
        }

        public void SendEmail(string fromName, string fromEmail, string toName, string toEmail, string subject, string textBody)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(fromName, fromEmail));
            message.To.Add(new MailboxAddress(toName, toEmail));
            message.Subject = subject;
            message.Body = new TextPart("plain") { Text = textBody };

            using var client = new SmtpClient();
            try
            {
                client.Connect(_smtpServer, _smtpPort, false);
                client.Authenticate(_smtpUser, _smtpPass);
                client.Send(message);
                _loggingHelper.LogInformation("Email sent successfully!");
            }
            catch (Exception ex)
            {
                _loggingHelper.LogError(ex, "An error occurred while sending the email.");
            }
            finally
            {
                client.Disconnect(true);
            }
        }
    }
}
