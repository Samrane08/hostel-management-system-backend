using AadhaarService.Model.Request;
using AadhaarService.Service;
using Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Model;
using NotificationService.Controllers;
using Repository.Entity;
using Repository.Interface;
using Service.Interface;
using System.Data;

namespace AadhaarService.Controllers
{
    public class VerifyOTPController : APIBaseController
    {
        private readonly IUIDService uIDService;
        private readonly IHttpClientService httpClientService;
        private readonly ICurrentUserService currentUserService;
        private readonly APIUrl urloptions;

        public VerifyOTPController(IUIDService uIDService, IHttpClientService httpClientService, ICurrentUserService currentUserService, IOptions<APIUrl> urloptions)
        {
            this.uIDService = uIDService;
            this.httpClientService = httpClientService;
            this.currentUserService = currentUserService;
            this.urloptions= urloptions.Value;
        }

        [HttpPost("verify-otp-anonymous")]
        [AllowAnonymous]
        public async Task<IActionResult> Post([FromBody] VerifyOTP model)
        {
            var result = await uIDService.VerifyOTPAsync(model.AadhaarNumber, model.OTP,model.OTPTxn);
            return Ok(result);
        }
        [HttpPost("verify-otp")]
        public async Task<IActionResult> VerifyOTP([FromBody] VerifyOTP model)
        {
            try
            {
                model.AadhaarNumber = AesAlgorithm.DecryptString(model.AadhaarNumber);
                var AadhaarResponse = await uIDService.VerifyOTPAsync(model.AadhaarNumber, model.OTP, model.OTPTxn);

                if (AadhaarResponse != null && !string.IsNullOrEmpty(AadhaarResponse.ReferenceNo))
                {
                    if (!string.IsNullOrWhiteSpace(AadhaarResponse.ApplicantImage_string))
                    {
                        try
                        {
                            var uidImage = Convert.FromBase64String(AadhaarResponse.ApplicantImage_string);
                            IFormFile formFile = new AadhaarImageFormFile(uidImage, $"{AadhaarResponse.ReferenceNo}.jpeg");
                            var aadhaarImageResponse = await httpClientService.FileUpload(formFile);
                            if (aadhaarImageResponse != null)
                                AadhaarResponse.ApplicantImage_string = aadhaarImageResponse.Id;
                        }
                        catch (Exception ex)
                        {
                            AadhaarResponse.ApplicantImage_string = null;

                        }
                    }
                    var m = new ApplicantAadharDetail();
                    m.UserId = Convert.ToInt64(currentUserService.UserNumericId);
                    m.UIDNo = AadhaarResponse.UIDNo;
                    m.UIDReference = Convert.ToInt64(AadhaarResponse.ReferenceNo);
                    m.ApplicantName = AadhaarResponse.ApplicantName;
                    m.ApplicantNameLL = AadhaarResponse.ApplicantName_LL;
                    m.DateOfBirth = AadhaarResponse.DateOfBirth;
                    m.Gender = AadhaarResponse.Gender;
                    m.Mobile = AadhaarResponse.MobileNo;
                    m.State = AadhaarResponse.StateName;
                    m.District = AadhaarResponse.DistrictName;
                    m.Taluka = AadhaarResponse.TalukaName;
                    m.Address = AadhaarResponse.Address;
                    m.Pincode = AadhaarResponse.Pincode;
                    m.AadhaarImage = AadhaarResponse.ApplicantImage_string;
                    m.CreatedBy = currentUserService.UserId;
                    string result = await uIDService.InsertApplicantAadharDetails(m);
                    if (result != null && result.Split(":")[1] == "EXISTS")
                    {
                        return Ok(new
                        {
                            Status = false,
                            Message = "Aadhaar alredy exists."

                        });
                    }
                    else
                    {
                        var updateloginresult = await httpClientService.RequestSend<UpdateloginModel>(HttpMethod.Get, $"{urloptions.UserService}/Account/Update-AadharStatus?IsAadharVerified=true&UserId={Convert.ToInt64(currentUserService.UserNumericId)}", null);
                        if (updateloginresult != null && updateloginresult.Status)
                        {
                            return Ok(new
                            {
                                Status = true,
                                Message = "Aadhaar Verified Successfully."

                            });
                        }
                        else
                        {
                            return Ok(new
                            {
                                Status = false,
                                Message = "Aadhaar Verified failed."
                            });

                        }
                    }
                }

                else
                {
                    return Ok(new
                    {
                        Status = false,
                        Message = "Aadhaar Verified failed."
                    });
                }
        }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(Convert.ToString(ex));
                return Ok(new
                {
                    Status = false,
                    Message = "Aadhaar Verified failed."
                });
            }
        }

        [HttpPost("forgot-username-verify-otp")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotUsernameVerifyOTP([FromBody] VerifyOTP model)
        {
            try
            {

                  model.AadhaarNumber = AesAlgorithm.DecryptString(model.AadhaarNumber);
                var AadhaarResponse = await uIDService.VerifyOTPAsync(model.AadhaarNumber, model.OTP, model.OTPTxn);
                if (AadhaarResponse != null && !string.IsNullOrEmpty(AadhaarResponse.ReferenceNo))
                {

                    var result = await uIDService.GetUserIdByAadharRefNo(Convert.ToInt64(AadhaarResponse.ReferenceNo));
                    var logingdetails = await httpClientService.RequestSend<Applicantdetails>(HttpMethod.Get, $"{urloptions.UserService}/Account/logindetails-ByUserId?userId={result.UserId}", null);
                    if (result != null && logingdetails is not null)
                    {
                        var paramList = new List<string>();
                        paramList.Add(result.ApplicantName);
                        paramList.Add(logingdetails.UserName);
                        var reqParam = new { key = "Forgot_Username", to = logingdetails.EmailId, param = paramList };
                        await httpClientService.RequestSend<object>(HttpMethod.Post, $"{urloptions.NotificationService}/Email/send-with-key", reqParam);
                        return Ok(new
                        {
                            Status = true,
                            Message = "Your username has been sent to registered Email Id"
                        });
                    }
                    else
                        return Ok(new
                        {
                            Status = false,
                            Message = "Username not found"
                        });
                }
                else
                    return Ok(new
                    {
                        Status = false,
                        Message = "Invalid OTP"
                    });
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(Convert.ToString(ex));
                return Ok(new
                {
                    Status = false,
                    ex.Message
                });
            }
        }
    }
}
