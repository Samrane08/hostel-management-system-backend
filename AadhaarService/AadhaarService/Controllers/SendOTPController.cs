using AadhaarService.Helper;
using AadhaarService.Model;
using Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NotificationService.Controllers;
using Service.Interface;

namespace AadhaarService.Controllers
{
    public class SendOTPController : APIBaseController
    {
        private readonly IUIDService uIDService;
        private readonly ICacheService cacheService;
        private readonly IBruteForceService bruteForceService;

        public SendOTPController(IUIDService uIDService, ICacheService cache, IBruteForceService bruteForceService)
        {
            this.uIDService = uIDService;
            this.cacheService = cache;
            this.bruteForceService = bruteForceService;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Post([FromBody] string AadhaarNumber)
        {
            //var ip_con = HttpContext.Connection.RemoteIpAddress?.ToString();


            //var isBruteForce = await bruteForceService.IsBruteForce(ip_con);
            //if (isBruteForce)
            //{
            //    bruteForceService.RegisterFailedAttempt(ip_con);
            //    return Ok(new { Status = false, Message = "Too many requests. You can raise next otp request after 5 minutes." });
            //}
            //else
            //{
            //    bruteForceService.RegisterFailedAttempt(ip_con);
            //}

            var dec = AesAlgorithm.DecryptString(AadhaarNumber);
            var result = await uIDService.SendOtpAsync(dec);

            
            return Ok(result);
            
        }

      
    }
}
