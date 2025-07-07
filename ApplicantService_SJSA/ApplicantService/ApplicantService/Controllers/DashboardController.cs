using Microsoft.AspNetCore.Mvc;
using Service.Interface;

namespace ApplicantService.Controllers
{
    public class DashboardController : APIBaseController
    {
        private readonly ILoginDetailsService loginDetailsService;

        public DashboardController(ILoginDetailsService loginDetailsService)
        {
            this.loginDetailsService = loginDetailsService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var CheckStatus = await loginDetailsService.VerifyStatus();

            if (CheckStatus != null && CheckStatus.IsAadharVerified && CheckStatus.IsEmailVerified && CheckStatus.IsMobileVerified)
            {  
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
