using ApplicantService.Helper;
using ApplicantService.Model;
using ApplicantService.Service;
using Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Model;
using Service.Interface;

namespace ApplicantService.Controllers
{
    public class AadhaarController : APIBaseController
    {
        private readonly IHttpClientService httpClientService;
        private readonly ILoginDetailsService loginDeatails;
        private readonly APIUrl urloptions;

        public AadhaarController(IHttpClientService httpClientService,
                                 IOptions<APIUrl> urloptions,
                                 ILoginDetailsService loginDeatails)
        {
            this.httpClientService = httpClientService;
            this.loginDeatails = loginDeatails;
            this.urloptions = urloptions.Value;
        }

        [HttpPost("verify-otp")]
        public async Task<IActionResult> VerifyOTP([FromBody] VerifyOTP model)
        {
            try
            {
                model.AadhaarNumber = AESDecryption.DecryptString(model.AadhaarNumber);
                var AadhaarResponse = await httpClientService.RequestSend<UIDResponseModel>(HttpMethod.Post, $"{urloptions.AadharService}/VerifyOTP", model);
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
                            ExceptionLogging.LogException(Convert.ToString(ex));
                        }
                    }
                    var result = await loginDeatails.UpsertAadhaarAsync(AadhaarResponse);
                    if (result != null && result.Message != "EXISTS")
                        return Ok(new
                        {
                            Status = true,
                            Message = "Aadhaar Verified Successfully."
                        });
                    else
                        return Ok(new
                        {
                            Status = false,
                            Message = "Aadhaar alredy exists."
                        });
                }
                else
                    return Ok(new
                    {
                        Status = false,
                        Message = "Aadhaar Verified failed."
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

        [HttpPost("forgot-username-verify-otp")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotUsernameVerifyOTP([FromBody] VerifyOTP model)
        {
            try
            {
              
                model.AadhaarNumber = AESDecryption.DecryptString(model.AadhaarNumber);
                var AadhaarResponse = await httpClientService.RequestSend<UIDResponseModel>(HttpMethod.Post, $"{urloptions.AadharService}/VerifyOTP", model);
                if (AadhaarResponse != null && !string.IsNullOrEmpty(AadhaarResponse.ReferenceNo))
                {
                   
                    var result = await loginDeatails.GetLoginDetails(AadhaarResponse.ReferenceNo);
                    if (result != null)
                    {
                        var paramList = new List<string>();
                        paramList.Add(result.FullName);
                        paramList.Add(result.UserName);
                        var reqParam = new { key = "Forgot_Username", to = result.EmailID, param = paramList };
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
