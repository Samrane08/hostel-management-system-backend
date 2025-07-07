using Helper;
using Microsoft.AspNetCore.Mvc;
using Model;
using Service.Interface;

namespace HostelService.Controllers
{
    public class NoticeController : APIBaseController
    {
        private readonly IWardenProfileService wardenProfile;

        public NoticeController(IWardenProfileService wardenProfile)
        {
            this.wardenProfile = wardenProfile;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var result = await wardenProfile.GetNotices();
                if (result != null)
                    return Ok(new { Status = true, Notices = result });
                else
                    return Ok(new { Status = false, Notices = result });
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(Convert.ToString(ex));
                return Ok(new { Status = false, message = ex.Message });
            }
        }
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ReqNoticeModel model)
        {
            try
            {
                var result = await wardenProfile.SaveNotices(model);
                if (result != null)
                    return Ok(new { Status = true, Message = "Notice Saved Successfully" });
                else
                    return Ok(new { Status = false, Message = "Notice Save Failed." });
            }
            catch (Exception ex)
            {
                return Ok(new { Status = false, message = ex.Message });
            }
        }
        [HttpPost("delete")]
        public async Task<IActionResult> Delete([FromBody] long Id)
        {
            try
            {
                var result = await wardenProfile.DeleteNitice(Id);
                if (result)
                {
                    return Ok(new { Status = true, Message = "Notice deleted successfully." });
                }
                else
                {
                   
                    return Ok(new { Status = false, Message = " deleted failed." });
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(Convert.ToString(ex));
                return Ok(new { Status = false, message = ex.Message });
            }
        }
    }
}
