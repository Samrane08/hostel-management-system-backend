using Microsoft.AspNetCore.Mvc;
using Service.Interface;

namespace ApplicantService.Controllers
{
    public class ProgressStatusController : APIBaseController
    {
        private readonly IProfileService profileService;
        public ProgressStatusController(IProfileService profileService)
        {
            this.profileService = profileService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await profileService.ProfileStatus());
        }
    }
}
