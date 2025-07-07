using Model;
namespace Service.Interface;
public interface IOtpVerifyService
{
    Task<OtpSendModel> SendOTPAsync(string Mail);
    Task<object> VerifyOTPAsync(string Mail, string OTP);
    Task<OtpSendModel> SendSMSOTPAsync(string Mobile);
    Task<object> VerifySMSOTPAsync(string Mobile, string OTP);
    Task<bool> IsMobileVerified(string Mobile);
    Task<bool> IsEmailVerified(string Email);
}
