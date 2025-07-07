using Helper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Model;
using Model.Common;
using Repository.Data;
using Service.Interface;
using System.Text.Json;

namespace Service.Implementation;
public class EmailLogger : IEmailLogger
{
    private readonly ILogger<EmailLogger> logger;
    private readonly ApplicationDbContext context;
    private readonly IEmailService emailService;
    public EmailLogger(ILogger<EmailLogger> logger,
                       ApplicationDbContext context,
                       IEmailService emailService)
    {
        this.logger = logger;
        this.context = context;
        this.emailService = emailService;
    }
    public async Task<string> Log(EmailBroadcast model)
    {
        try
        {
            Repository.Entity.EmailSender sender = new Repository.Entity.EmailSender
            {
                To = model.to,
                Cc = model.cc,
                Bcc = model.bcc,
                Subject = model.subject,
                Body = model.body,
                SendStatus = false
            };
            await context.EmailSender.AddAsync(sender);
            await context.SaveChangesAsync();
            return sender.Id;
        }
        catch (Exception ex)
        {

            ExceptionLogging.LogException(Convert.ToString(ex));
            logger.LogError(ex, $"Email Track Error Log({JsonSerializer.Serialize(model)})");
            throw;
        }
    }
    public async Task Log(string TrackId)
    {
        try
        {
            var result = await context.EmailSender.Where(x => x.Id == TrackId).FirstOrDefaultAsync();
            if (result != null)
            {
                result.SendAt = DateTime.Now;
                result.SendStatus = await emailService.SendAsync(new EmailParam
                {
                    To = result?.To?.Split(",").Select(email => email.Trim()).ToList(),
                    CC = result?.Cc?.Split(",").Select(email => email.Trim()).ToList(),
                    Body = result?.Body,
                    Subject = result?.Subject,
                });
                await context.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {

            ExceptionLogging.LogException(Convert.ToString(ex));
            logger.LogError(ex, $"Email Track Error Log({TrackId})");
            throw;
        }
    }
    public async Task<string> EmailSend(EmailSender2 model)
    {
        try
        {
            var template = await context.EmailTemplates.Where(x => x.Key == model.key).FirstOrDefaultAsync();
            if (template != null)
            {
                var body = template.Body;
                var Cnt = model.param?.Count;
                switch (Cnt)
                {
                    case 1:
                        body = template.Body?.Replace("#var1", model.param?[0]);
                        break;
                    case 2:
                        body = template.Body?.Replace("#var1", model.param?[0]).Replace("#var2", model.param?[1]);
                        break;
                    case 3:
                        body = template.Body?.Replace("#var1", model.param?[0]).Replace("#var2", model.param?[1]).Replace("#var3", model.param?[2]);
                        break;
                    case 4:
                        body = template.Body?.Replace("#var1", model.param?[0]).Replace("#var2", model.param?[1]).Replace("#var3", model.param?[2]).Replace("#var4", model.param?[3]);
                        break;
                    case 5:
                        body = template.Body?.Replace("#var1", model.param?[0]).Replace("#var2", model.param?[1]).Replace("#var3", model.param?[2]).Replace("#var4", model.param?[3]).Replace("#var5", model.param?[4]);
                        break;
                    case 6:
                        body = template.Body?.Replace("#var1", model.param?[0]).Replace("#var2", model.param?[1]).Replace("#var3", model.param?[2]).Replace("#var4", model.param?[3]).Replace("#var5", model.param?[4]).Replace("#var6", model.param?[5]);
                        break;
                    case 7:
                        body = template.Body?.Replace("#var1", model.param?[0]).Replace("#var2", model.param?[1]).Replace("#var3", model.param?[2]).Replace("#var4", model.param?[3]).Replace("#var5", model.param?[4]).Replace("#var6", model.param?[5]).Replace("#var7", model.param?[6]);
                        break;
                    case 8:
                        body = template.Body?.Replace("#var1", model.param?[0]).Replace("#var2", model.param?[1]).Replace("#var3", model.param?[2]).Replace("#var4", model.param?[3]).Replace("#var5", model.param?[4]).Replace("#var6", model.param?[5]).Replace("#var7", model.param?[6]).Replace("#var8", model.param?[7]);
                        break;
                    default:
                        break;
                }
                Repository.Entity.EmailSender sender = new Repository.Entity.EmailSender
                {
                    To = model.to,
                    Subject = template.Subject,
                    Body = body,
                    SendStatus = false
                };
                await context.EmailSender.AddAsync(sender);
                await context.SaveChangesAsync();
                return sender.Id;
            }
            else
                return "";
        }
        catch (Exception ex)
        {

            ExceptionLogging.LogException(Convert.ToString(ex));
            logger.LogError(ex, $"Error at Email Send with key ({model.key})");
            return "";
        }
    }
}