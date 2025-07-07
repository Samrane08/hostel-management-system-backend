using Helper;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using Model.Common;
using Service.Interface;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace Service.Implementation;

public class EmailService : IEmailService
{
    private readonly EmailSetting mailSettings;
    private readonly IExceptionLogger logger;
    public EmailService(IOptions<EmailSetting> mailSettings,IExceptionLogger logger)
    {
        this.mailSettings = mailSettings.Value;
        this.logger = logger;
    }
    public async Task<bool> SendAsync(EmailParam param)
    {
        try
        {
            var multipart = new Multipart("mixed");
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress("No-Reply", mailSettings.Name));
            //email.Sender = MailboxAddress.Parse(mailSettings.EMail);           
            if (param.To != null)
            {
                foreach (var item in param.To)
                {
                    if (!string.IsNullOrWhiteSpace(item))
                        email.To.Add(MailboxAddress.Parse(item));
                }
            }

            if (param.CC != null)
            {
                foreach (var item in param.CC)
                {
                    if (!string.IsNullOrWhiteSpace(item))
                        email.Cc.Add(MailboxAddress.Parse(item));
                }
            }           
            email.Subject = param.Subject;
            var builder = new BodyBuilder();
            if (param.Files != null)
            {
                byte[] fileBytes;
                foreach (var file in param.Files)
                {
                    if (file.Length > 0)
                    {
                        using (var ms = new MemoryStream())
                        {
                            file.CopyTo(ms);
                            fileBytes = ms.ToArray();
                        }
                        builder.Attachments.Add(file.FileName, fileBytes, ContentType.Parse(file.ContentType));
                    }
                }
            }
            builder.HtmlBody = param.Body;
            email.Body = builder.ToMessageBody();
            multipart.Add(email.Body);
            email.Body = multipart;
            ServicePointManager.ServerCertificateValidationCallback += ValidateServerCertificate;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                                                 | SecurityProtocolType.Tls11
                                                 | SecurityProtocolType.Tls12
                                                 | SecurityProtocolType.Tls13;
            using var smtp = new MailKit.Net.Smtp.SmtpClient();
            smtp.Connect(mailSettings.Host, mailSettings.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(mailSettings.EMail, mailSettings.Password);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
            return true;
        }
        catch (Exception ex)
        {

            ExceptionLogging.LogException(Convert.ToString(ex));
            await logger.LogTrack("Email Sender", ex.Message);
            return false;
        }
    }

    private static bool ValidateServerCertificate(
        object sender,
        X509Certificate certificate,
        X509Chain chain,
        SslPolicyErrors sslPolicyErrors)
    {
        //if (sslPolicyErrors == SslPolicyErrors.None)
          //  return true;

        // Add custom validation logic if necessary
        return true;
    }
}
