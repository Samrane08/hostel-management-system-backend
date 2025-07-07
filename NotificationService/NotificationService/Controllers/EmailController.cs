using Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Model;
using NotificationService.Service;
using Service.Interface;
namespace NotificationService.Controllers;
public class EmailController : APIBaseController
{
    private readonly IHubContext<NotifyHub> hub;
    private readonly IEmailLogger emailLogger;
    private readonly IScheduledMailTriggerService scheduledMailTriggerService;

    public EmailController(IHubContext<NotifyHub> hub,IEmailLogger emailLogger,IScheduledMailTriggerService scheduledMailTriggerService)
    {
        this.hub = hub;
        this.emailLogger = emailLogger;
        this.scheduledMailTriggerService = scheduledMailTriggerService;
    }
    
    [HttpPost]
    public async Task<IActionResult> Post([FromBody]EmailBroadcast  model)
    {
        try
        {
            var NotificationId = await emailLogger.Log(model);
            await hub.Clients.All.SendAsync("EmailNotification", NotificationId, (object)model);
            return Ok(new { Status = true, Message = "Email Send Successfully." });
        }
        catch (Exception ex)
        {
            ExceptionLogging.LogException(Convert.ToString(ex));
            return Ok(new { Status = false, Message = ex.Message });
        }
    }

    [HttpPost("send-with-key")]
    [AllowAnonymous]
    public async Task<IActionResult> Send([FromBody] EmailSender2 model)
    {
        try
        {
            var NotificationId = await emailLogger.EmailSend(model);
            await hub.Clients.All.SendAsync("EmailNotification", NotificationId, (object)model);
            return Ok(new { Status = true, Message = "Email Send Successfully." });
        }
        catch (Exception ex)
        {

            ExceptionLogging.LogException(Convert.ToString(ex));
            return Ok(new { Status = false, Message = ex.Message });
        }
    }

    [HttpPost("schedule-send")]
    [AllowAnonymous]
    public async Task<IActionResult> ScheduleSend([FromBody] EmailSender3 model)
    {
        try
        {
            await scheduledMailTriggerService.EmailSend(model);           
            return Ok(new { Status = true, Message = "Email Scheduled." });
        }
        catch (Exception ex)
        {

            ExceptionLogging.LogException(Convert.ToString(ex));
            return Ok(new { Status = false, Message = ex.Message });
        }
    }


    [HttpPost("brodcast")]
    [AllowAnonymous]
    public async Task<IActionResult> Brodcast([FromBody] string TrackId)
    {
        await emailLogger.Log(TrackId);
        return NoContent();
    }
}
