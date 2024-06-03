using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;
using System.IO;

public class EmailService
{
    private readonly IConfiguration _configuration;

    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void SendEmail(string toEmail, string subject, string body, string imagePath = null)
    {
        var smtpSettings = _configuration.GetSection("Smtp");
        var smtpClient = new SmtpClient(smtpSettings["Host"])
        {
            Port = int.Parse(smtpSettings["Port"]),
            Credentials = new NetworkCredential(smtpSettings["UserName"], smtpSettings["Password"]),
            EnableSsl = bool.Parse(smtpSettings["EnableSsl"])
        };

        var mailMessage = new MailMessage
        {
            From = new MailAddress(smtpSettings["UserName"]),
            Subject = subject,
            Body = body,
            IsBodyHtml = true
        };

        mailMessage.To.Add(toEmail);

        if (!string.IsNullOrEmpty(imagePath) && File.Exists(imagePath))
        {
            Attachment attachment = new Attachment(imagePath);
            mailMessage.Attachments.Add(attachment);
        }

        smtpClient.Send(mailMessage);
    }
}
