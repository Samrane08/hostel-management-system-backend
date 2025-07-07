using Helper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Model;
using Model.Common;
using Repository.Data;
using Repository.Entity;
using Service.Interface;
using System.Text.Json;

namespace Service.Implementation;
public class SMSLogger: ISMSLogger
{
    private readonly ILogger<SMSLogger> logger;
    private readonly ApplicationDbContext context;
    private readonly ISMSService sMSService;

    public SMSLogger(ILogger<SMSLogger> logger,
                       ApplicationDbContext context,
                       ISMSService sMSService)
    {
        this.logger = logger;
        this.context = context;
        this.sMSService = sMSService;
    }

    public async Task<string> Log(SMSBroadcast model)
    {
        try
        {
            SMSSender sender = new SMSSender
            {
                To = model.mobile,
                TemplateId = model.templateid,
                Content = model.body,
                CreatedOn = DateTime.Now,                
                SendStatus = false
            };
            await context.SMSSender.AddAsync(sender);
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
            var result = await context.SMSSender.Where(x => x.Id == TrackId).FirstOrDefaultAsync();
            if (result != null)
            {
                var sendStatus = await sMSService.SendSMS(new SMSParam{ Templateid = result?.TemplateId, Mobile = result?.To, Body = result?.Content});
                result.SendAt = DateTime.Now;
                result.SendStatus = sendStatus;
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
}
