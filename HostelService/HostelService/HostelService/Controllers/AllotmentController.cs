using HostelService.Helper;
using HostelService.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Model;
using Repository.Interface;
using Service.Implementation;
using Service.Interface;

namespace HostelService.Controllers
{
    public class AllotmentController : APIBaseController
    {
        private readonly IAllotmentService allotmentService;
        private readonly IHttpClientService httpClientService;
        private readonly APIUrl urloptions;
        private readonly ICurrentUserService currentUserService;
        public AllotmentController(IAllotmentService allotmentService, IHttpClientService httpClientService, IOptions<APIUrl> urloptions, ICurrentUserService currentUserService)
        {
            this.allotmentService = allotmentService;
            this.httpClientService = httpClientService;
            this.urloptions = urloptions.Value;
            this.currentUserService = currentUserService;
        }
        [HttpGet("by-course")]
        public async Task<IActionResult> ByCourse(int? HostelId, int? CourseId)
        {
            var result = await allotmentService.GetCourseWiseList(HostelId, CourseId);

            if (result.Count>0)
                return Ok(new { Status = true, Data = result });
            
            else
                return Ok(new { Status = false, Message = "No allocation for this hostel!!"      });

            // return Ok(await allotmentService.GetCourseWiseList(HostelId, CourseId));
        }
        [HttpGet("by-category")]
        public async Task<IActionResult> ByCategory(int? HostelId, int? CourseId)
        {
            return Ok(await allotmentService.GetCategoryWiseList(HostelId, CourseId));
        }
        [HttpGet("by-caste")]
        public async Task<IActionResult> ByCaste(int? HostelId, int? CourseId, int? CasteId)
        {
            return Ok(await allotmentService.GetCasteWiseList(HostelId, CourseId, CasteId));
        }

        [HttpPost]
        public async Task<IActionResult> PendingList([FromBody] SearchAllotmentStatusModel model)
        {
            return Ok(await allotmentService.GetListAsync(model));
        }
        [HttpPost("list")]
        public async Task<IActionResult> List([FromBody] SearchAllotmentStatusModel model)
        {
            return Ok(await allotmentService.GetAllListAsync(model));
        }

        [HttpGet("hostel-list")]
        [AllowAnonymous]
        public async Task<IActionResult> HostelList()
        {
            var response = new List<SelectListModel>();
            response = await allotmentService.GetHostelList();

            return Ok(response);
        }

        [HttpGet("hostel-list-distict-wise-delete-sjsa-aadhaar")]
        [AllowAnonymous]
        public async Task<IActionResult> HostelListDistrictWiseDeleteSjsaAadhaar(int? DistrictId)
        {
            var response = new List<SelectListModel>();
            response = await allotmentService.GetHostelListDistrictWiseDeleteSjsaAadhaar(DistrictId);

            return Ok(response);
        }

    }
}
