using Model;
using Repository.Entity;

namespace Service.Interface;

public interface IUIDService
{
    Task<AadharRes> SendOtpAsync(string AadhaarPlainText);
    Task<AadharResponseResult> VerifyOTPAsync(string AadhaarPlainText, string OTP, string OTPTxn);
    string GetUIDReference(string AadhaarPlainText);
    Task<string>InsertApplicantAadharDetails(ApplicantAadharDetail AadhaarPlainText);

    Task<PartialAadharDetails> GetUserIdByAadharRefNo(long AadharRefNo);
    Task<bool> UpdateAadharDOB(string DOB);
}