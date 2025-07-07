using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using NotificationService.Model.Request;
using NotificationService.Service;
using Service.Interface;

namespace NotificationService.Controllers;

public class OTPController : APIBaseController
{
    private readonly IOtpVerifyService otpVerifyService;
    private readonly IHubContext<NotifyHub> hub;
    private readonly ISMSLogger smslLogger;
    private readonly IEmailLogger emailLogger;

    public OTPController(IOtpVerifyService otpVerifyService, IHubContext<NotifyHub> hub, ISMSLogger smslLogger, IEmailLogger emailLogger)
    {
        this.otpVerifyService = otpVerifyService;
        this.hub = hub;
        this.smslLogger = smslLogger;
        this.emailLogger = emailLogger;
    }

    [HttpPost("Email/[action]")]
    [AllowAnonymous]
    public async Task<IActionResult> Send([FromBody] string Email)
    {
        var response = await otpVerifyService.SendOTPAsync(Email);
        if (response.Status)
        {           
            await hub.Clients.All.SendAsync("EmailNotification", response.SenderId, new { message = "SignalR broadcasted." });
        }
        return Ok(new { Status = response.Status,Message = response.Message });
    }

    [HttpPost("Email/[action]")]
    [AllowAnonymous]
    public async Task<IActionResult> Verify([FromBody] EmailVerifyModel model)
    {
        return Ok(await otpVerifyService.VerifyOTPAsync(model.Email, model.OTP));
    }
    [HttpPost("SMS/Send")]
    [AllowAnonymous]
    public async Task<IActionResult> SMSSend([FromBody] string Mobile)
    {
        var response = await otpVerifyService.SendSMSOTPAsync(Mobile);
        if (response.Status)
        {
            await hub.Clients.All.SendAsync("SMSNotification",response.SenderId, new { message = "SignalR broadcasted." });
        }
        return Ok(new { Status = response.Status,Message = response.Message });
    }

    [HttpPost("SMS/Verify")]
    [AllowAnonymous]
    public async Task<IActionResult> SMSVerify([FromBody] SMSVerifyModel model)
    {
        return Ok(await otpVerifyService.VerifySMSOTPAsync(model.Mobile, model.OTP));
    }

    [HttpPost("is-mobile-verified")]
    [AllowAnonymous]
    public async Task<IActionResult> ISMobileVerified([FromBody] string Mobile)
    {
        return Ok(await otpVerifyService.IsMobileVerified(Mobile));
    }

    [HttpPost("is-email-verified")]
    [AllowAnonymous]
    public async Task<IActionResult> ISEmailVerified([FromBody] string Email)
    {
        return Ok(await otpVerifyService.IsEmailVerified(Email));
    }
}
