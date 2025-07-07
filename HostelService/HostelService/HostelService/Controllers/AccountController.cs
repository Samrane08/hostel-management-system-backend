using Helper;
using HostelService.Helper;
using HostelService.Model;
using HostelService.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Model;
using Service.Interface;

namespace HostelService.Controllers
{
    public class AccountController : APIBaseController
    {
        private readonly IAccountService accountService;
        private readonly IHttpClientService httpClientService;
        private readonly APIUrl urloptions;
        public AccountController(IAccountService accountService, IHttpClientService httpClientService, IOptions<APIUrl> urloptions)
        {
            this.accountService = accountService;
            this.httpClientService = httpClientService;
            this.urloptions = urloptions.Value;
        }
        [HttpGet("{Id}/{deptId}")]
        [AllowAnonymous]
        public async Task<IActionResult> Get(string Id,int?deptId)
        {
            return Ok(await accountService.GetHostelProfile(Id, deptId));
        }

        [HttpGet("Department/{Id}/{EntityRole}/{deptId}")]
        [AllowAnonymous]
        public async Task<IActionResult> Department(string Id,int? EntityRole, int? deptId)
        {
            return Ok(await accountService.GetDepartmentDetails(Id, EntityRole, deptId));
        }

        [HttpPost("verify-aadhaar-otp")]
        public async Task<IActionResult> VerifyOTP([FromBody] VerifyOTP model)
        {
            try
            {
                var AadhaarResponse = await httpClientService.RequestSend<UIDResponseModel>(HttpMethod.Post, $"{urloptions.AadharService}/VerifyOTP", model);
                if (AadhaarResponse != null && !string.IsNullOrEmpty(AadhaarResponse.ReferenceNo))
                {
                    int Gender = 0;                    
                    if (string.Equals(AadhaarResponse.Gender,"Male")) Gender = 1;
                    if (string.Equals(AadhaarResponse.Gender, "Female")) Gender = 2;
                    if (string.Equals(AadhaarResponse.Gender, "Transgender")) Gender = 3;
                    var result = await accountService.UpdateAadhaar(AadhaarResponse.ReferenceNo, AadhaarResponse.ApplicantName, Gender);
                    if (result)
                        return Ok(new
                        {
                            Status = true,
                            Message = "Aadhaar Verified Successfully."
                        });
                    else
                        return Ok(new
                        {
                            Status = false,
                            Message = "Aadhaar Verified failed."
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

        [HttpPost("update-first-login")]
        [AllowAnonymous]
        public async Task<IActionResult> UpdateFirstLogin([FromBody]string Id)
        {
            return Ok(await accountService.UpdateFirstLogin(Id));
        }
        [HttpPost("update-first-login-reset-password")]
        [AllowAnonymous]
        public async Task<IActionResult> UpdateFirstLoginResetPassword([FromBody] string userId)
        {
            return Ok(await accountService.UpdateFirstLoginToTrue(userId));
        }
    }
} 
