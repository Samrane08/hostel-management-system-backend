using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model;
using Service.Interface;

namespace HostelService.Controllers
{
    public class HostelController : APIBaseController
    {
        private readonly IHostelProfileService hostelProfileService;

        public HostelController(IHostelProfileService hostelProfileService)
        {
            this.hostelProfileService = hostelProfileService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await hostelProfileService.GetHostelProfile();

            if (result != null)
                return Ok(new { Status = true, HotelProfile = result });
            else
                return Ok(new { Status = false, Message = "Hostel Profile not found." });
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] HostelProfileModel model)
        {
            var result = await hostelProfileService.SaveHostelProfile(model);

            if (result != null)
                return Ok(new { Status = true, Message = "Hostel Profile Save Successfully." });
            else
                return Ok(new { Status = false, Message = "Hostel Profile Save Failed." });
        }
    }
}
