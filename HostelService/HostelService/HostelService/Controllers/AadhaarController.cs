using Helper;
using HostelService.Helper;
using HostelService.Model;
using HostelService.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Model;

namespace HostelService.Controllers
{
    public class AadhaarController : APIBaseController
    {
        private readonly IHttpClientService httpClientService;
        private readonly APIUrl urloptions;

        public AadhaarController(IHttpClientService httpClientService,
                                 IOptions<APIUrl> urloptions)
        {
            this.httpClientService = httpClientService;
            this.urloptions = urloptions.Value;
        }

        [HttpPost("first-login-verify-otp")]
        public async Task<IActionResult> ForgotUsernameVerifyOTP([FromBody] VerifyOTP model)
        {
            try
            {

                model.AadhaarNumber = AESDecryption.DecryptString(model.AadhaarNumber);
                var AadhaarResponse = await httpClientService.RequestSend<UIDResponseModel>(HttpMethod.Post, $"{urloptions.AadharService}/VerifyOTP", model);
                if (AadhaarResponse != null && !string.IsNullOrEmpty(AadhaarResponse.ReferenceNo))
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
                };
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
