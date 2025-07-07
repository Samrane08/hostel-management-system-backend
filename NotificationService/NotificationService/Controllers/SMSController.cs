using Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Model;
using NotificationService.Service;
using Service.Interface;

namespace NotificationService.Controllers;
public class SMSController : APIBaseController
{
    private readonly IHubContext<NotifyHub> hub;
    private readonly ISMSLogger smslLogger;
    public SMSController(IHubContext<NotifyHub> hub, ISMSLogger smslLogger)
    {
        this.hub = hub;
        this.smslLogger = smslLogger;
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] SMSBroadcast model)
    {
        try
        {
            var NotificationId = await smslLogger.Log(model);
            await hub.Clients.All.SendAsync("SMSNotification", NotificationId, (object)model);
            return Ok(new { Status = true, Message = "SMS Send Successfully." });
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
        await smslLogger.Log(TrackId);
        return NoContent();
    }
}
