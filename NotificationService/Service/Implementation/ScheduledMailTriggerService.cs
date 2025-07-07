using Helper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Model;
using Model.Common;
using Repository.Data;
using Service.Interface;

namespace Service.Implementation;

public class ScheduledMailTriggerService : IScheduledMailTriggerService
{
    private readonly ILogger<EmailLogger> logger;
    private readonly ApplicationDbContext context;
    private readonly IEmailService emailService;
    public ScheduledMailTriggerService(ILogger<EmailLogger> logger,
                       ApplicationDbContext context,
                       IEmailService emailService)
    {
        this.logger = logger;
        this.context = context;
        this.emailService = emailService;
    }

    public async Task EmailSend(EmailSender3 model)
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
                Repository.Entity.ScheduledMailTrigger sender = new Repository.Entity.ScheduledMailTrigger
                {
                    To = model.to,
                    Subject = template.Subject,
                    Body = body,
                    ScheduledAt = model.ScheduledAt,
                    SendStatus = false
                };
                await context.ScheduledMailTrigger.AddAsync(sender);
                await context.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {

            ExceptionLogging.LogException(Convert.ToString(ex));
            logger.LogError(ex, $"Error at Email Send with key ({model.key})");
        }
    }
    public async Task ScheduledMailTrigger()
    {
        try
        {
            var emails = await context.ScheduledMailTrigger
                                      .Where(x => x.ScheduledAt <= DateTime.Now && (x.SendStatus == false || x.SendStatus == null))
                                      .ToListAsync();

            if (emails.Count > 0)

                foreach (var email in emails)
                {
                    email.SendAt = DateTime.Now;
                    email.SendStatus = await emailService.SendAsync(new EmailParam
                    {
                        To = email?.To?.Split(",").Select(email => email.Trim()).ToList(),
                        CC = email?.Cc?.Split(",").Select(email => email.Trim()).ToList(),
                        Body = email?.Body,
                        Subject = email?.Subject,
                    });
                    await context.SaveChangesAsync();
                }
        }
        catch (Exception ex)
        {

            ExceptionLogging.LogException(Convert.ToString(ex));
            logger.LogError(ex, $"Error at Scheduled Mail Trigger");
        }
    }
}