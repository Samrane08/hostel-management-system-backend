using Helper;
using Microsoft.EntityFrameworkCore;
using Model;
using Repository.Data;
using Repository.Entity;
using Service.Interface;

namespace Service.Implementation;
public class OtpVerifyService : IOtpVerifyService
{
    private readonly ApplicationDbContext context;
    private readonly IEmailLogger emailLogger;
    private readonly ISMSLogger sMSLogger;
    public OtpVerifyService(ApplicationDbContext context, IEmailLogger emailLogger, ISMSLogger sMSLogger)
    {
        this.context = context;
        this.emailLogger = emailLogger;
        this.sMSLogger = sMSLogger;
    }
    public async Task<OtpSendModel> SendOTPAsync(string Mail)
    {
        var response = new OtpSendModel();
        try
        {
            var template = await context.EmailTemplates.Where(x => x.Key == "Email_Verify").FirstOrDefaultAsync();
            if (template != null)
            {
                var data = await context.OTPs.Where(x => x.User == Mail && x.IsVerified == null).OrderBy(x => x.ValidFrom).LastOrDefaultAsync();

                if (data != null)
                {
                    if (data.Attempt <= 3)
                    {
                        data.Otp = Utility.CreateOtp().ToString();
                        data.Attempt = data.Attempt + 1;
                        data.ValidFrom = DateTime.Now;
                        data.ValidTo = DateTime.Now.AddMinutes(3);

                        var reult = await emailLogger.Log(new EmailBroadcast
                        {
                            to = Mail,
                            subject = template.Subject,
                            body = template?.Body?.Replace("#var1", data.Otp)
                        });
                        await context.SaveChangesAsync();
                        response.Status = true;
                        response.SenderId = reult;
                        response.Message = "OTP Send";
                    }
                    else
                    {
                        var exipiryTime = DateTime.Now.Subtract(data.ValidTo).TotalMinutes;
                        if (exipiryTime >= 3)
                        {
                            data.Otp = Utility.CreateOtp().ToString();
                            data.Attempt = 1;
                            data.ValidFrom = DateTime.Now;
                            data.ValidTo = DateTime.Now.AddMinutes(3);
                            var reult = await emailLogger.Log(new EmailBroadcast
                            {
                                to = Mail,
                                subject = template.Subject,
                                body = template?.Body?.Replace("#var1", data.Otp)
                            });
                            await context.SaveChangesAsync();
                            response.Status = true;
                            response.SenderId = reult;
                            response.Message = "OTP Send.";
                        }
                        else
                        {
                            response.Status = false;
                            response.Message = "Maximum attempt reach.Try after some time.";
                        }
                    }
                }
                else
                {
                    var _otpdata = new OTP()
                    {
                        User = Mail,
                        Otp = Utility.CreateOtp().ToString(),
                        ValidFrom = DateTime.Now,
                        Attempt = 1,
                        ValidTo = DateTime.Now.AddMinutes(3),
                        Status = Repository.Enums.Status.Active
                    };
                    var reult = await emailLogger.Log(new EmailBroadcast
                    {
                        to = Mail,
                        subject = template.Subject,
                        body = template?.Body?.Replace("#var1", _otpdata.Otp)
                    });
                    await context.OTPs.AddAsync(_otpdata);
                    await context.SaveChangesAsync();
                    response.Status = true;
                    response.SenderId = reult;
                    response.Message = "OTP Send.";
                }
            }
            else
            {
                response.Status = true;
                response.Message = "Email Configuration needed.";
            }
        }
        catch (Exception ex)
        {

            ExceptionLogging.LogException(Convert.ToString(ex));
            response.Status = true;
            response.Message = ex.Message;
        }
        return response;
    }
    public async Task<object> VerifyOTPAsync(string Mail, string OTP)
    {
        try
        {
            var otpData = await context.OTPs.Where(x => x.User == Mail && x.Otp == OTP && (x.IsVerified == null || x.IsVerified == false)).FirstOrDefaultAsync();
            if (otpData != null)
            {
                if (otpData.ValidTo > DateTime.Now)
                {
                    otpData.VerifiedTime = DateTime.Now;
                    otpData.IsVerified = true;
                    await context.SaveChangesAsync();
                    return new { Status = true, Message = "OTP Verified." };
                }
                else
                    return new { Status = true, Message = "OTP Expired." };
            }
            else
                return new { Status = true, Message = "Invalid OTP." };
        }
        catch (Exception ex)
        {

            ExceptionLogging.LogException(Convert.ToString(ex));

            return new { Status = false, ex.Message };
        }
    }
    public async Task<OtpSendModel> SendSMSOTPAsync(string Mobile)
    {
        var response = new OtpSendModel();
        try
        {
            var template = await context.SMSTemplates.Where(x => x.Key == "Mobile_Verify").FirstOrDefaultAsync();
            if (template != null)
            {
                var data = await context.OTPs.Where(x => x.User == Mobile && x.IsVerified == null).OrderBy(x => x.ValidFrom).LastOrDefaultAsync();

                if (data != null)
                {
                    if (data.Attempt <= 3)
                    {
                        data.Otp = Utility.CreateOtp().ToString();
                        data.Attempt = data.Attempt + 1;
                        data.ValidFrom = DateTime.Now;
                        data.ValidTo = DateTime.Now.AddMinutes(3);

                        var reult = await sMSLogger.Log(new SMSBroadcast
                        {
                            mobile = Mobile,
                            templateid = template.TemplateId,
                            body = template?.Content?.Replace("#var1", data.Otp)
                        });
                        await context.SaveChangesAsync();
                        response.Status = true;
                        response.SenderId = reult;
                        response.Message = "OTP Send";
                    }
                    else
                    {
                        var exipiryTime = DateTime.Now.Subtract(data.ValidTo).TotalMinutes;
                        if (exipiryTime >= 3)
                        {
                            data.Otp = Utility.CreateOtp().ToString();
                            data.Attempt = 1;
                            data.ValidFrom = DateTime.Now;
                            data.ValidTo = DateTime.Now.AddMinutes(3);
                            var reult = await sMSLogger.Log(new SMSBroadcast
                            {
                                mobile = Mobile,
                                templateid = template.TemplateId,
                                body = template?.Content?.Replace("#var1", data.Otp)
                            });
                            await context.SaveChangesAsync();
                            response.Status = true;
                            response.SenderId = reult;
                            response.Message = "OTP Send.";
                        }
                        else
                        {
                            response.Status = false;
                            response.Message = "Maximum attempt reach.Try after some time.";
                        }
                    }
                }
                else
                {
                    var _otpdata = new OTP()
                    {
                        User = Mobile,
                        Otp = Utility.CreateOtp().ToString(),
                        ValidFrom = DateTime.Now,
                        Attempt = 1,
                        ValidTo = DateTime.Now.AddMinutes(3),
                        Status = Repository.Enums.Status.Active
                    };
                    var reult = await sMSLogger.Log(new SMSBroadcast
                    {
                        mobile = Mobile,
                        templateid = template.TemplateId,
                        body = template?.Content?.Replace("#var1", _otpdata.Otp)
                    });
                    await context.OTPs.AddAsync(_otpdata);
                    await context.SaveChangesAsync();
                    response.Status = true;
                    response.SenderId = reult;
                    response.Message = "OTP Send.";
                }
            }
            else
            {
                response.Status = true;
                response.Message = "SMS Configuration needed.";
            }
        }
        catch (Exception ex)
        {

            ExceptionLogging.LogException(Convert.ToString(ex));
            response.Status = true;
            response.Message = ex.Message;
        }
        return response;
    }
    public async Task<object> VerifySMSOTPAsync(string Mobile, string OTP)
    {
        try
        {
            var otpData = await context.OTPs.Where(x => x.User == Mobile && x.Otp == OTP && (x.IsVerified == null || x.IsVerified == false)).FirstOrDefaultAsync();
            if (otpData != null)
            {
                if (otpData.ValidTo > DateTime.Now)
                {
                    otpData.VerifiedTime = DateTime.Now;
                    otpData.IsVerified = true;
                    await context.SaveChangesAsync();
                    return new { Status = true, Message = "OTP Verified." };
                }
                else
                    return new { Status = true, Message = "OTP Expired." };
            }
            else
                return new { Status = true, Message = "Invalid OTP." };
        }
        catch (Exception ex)
        {

            ExceptionLogging.LogException(Convert.ToString(ex));
            return new { Status = false, ex.Message };
        }
    }
    public async Task<bool> IsMobileVerified(string Mobile)
    {
        try
        {
            var otpData = await context.OTPs.Where(x => x.User == Mobile && x.VerifiedTime >= DateTime.Now.AddMinutes(-5)).FirstOrDefaultAsync();
            if (otpData != null)
            {
                if(otpData.IsVerified != null)
                {
                    return (bool)otpData.IsVerified;
                }
                else
                    return true;
            }
            else
                return true;
        }
        catch (Exception ex)
        {

            ExceptionLogging.LogException(Convert.ToString(ex));
            return false;
        }
    }
    public async Task<bool> IsEmailVerified(string Email)
    {
        try
        {
            var otpData = await context.OTPs.Where(x => x.User == Email && x.VerifiedTime >= DateTime.Now.AddMinutes(-5)).FirstOrDefaultAsync();
            if (otpData != null)
            {
                if (otpData.IsVerified != null)
                {
                    return (bool)otpData.IsVerified;
                }
                else
                    return true;
            }
            else
                return true;
        }
        catch (Exception ex)
        {

            ExceptionLogging.LogException(Convert.ToString(ex));
            return false;
        }
    }
}

