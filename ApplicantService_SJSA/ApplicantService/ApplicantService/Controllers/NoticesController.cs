using Microsoft.AspNetCore.Mvc;
using Service.Interface;

namespace ApplicantService.Controllers
{

    public class NoticesController : APIBaseController
    {
        private readonly INoticeService noticesService;

        public NoticesController(INoticeService noticesService)
        {
            this.noticesService = noticesService;   
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await noticesService.GetNotices();
            return Ok(result);
        }
    }
}
