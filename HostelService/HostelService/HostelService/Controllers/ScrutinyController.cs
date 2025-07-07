using Microsoft.AspNetCore.Mvc;
using Model;
using Service.Interface;

namespace HostelService.Controllers
{
    public class ScrutinyController : APIBaseController
    {
        private readonly IScrutinyService scrutinyService;
        private readonly IApplicationService applicationService;

        public ScrutinyController(IScrutinyService scrutinyService,IApplicationService applicationService)
        {
            this.scrutinyService = scrutinyService;
            this.applicationService = applicationService;
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> Get(long Id)
        {
            return Ok(new { Status = true, Data = await scrutinyService.Workflow(Id)});
        }

        [HttpGet("History/{Id}")]        
        public async Task<IActionResult> History(long Id)
        {
            var IsValid = await applicationService.ApplicationValidate(Id);
            if (!IsValid)
            {
                return BadRequest(new { Message = "No data found." });
            }
            return Ok(new { Status = true, Data = await scrutinyService.History(Id)});
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ScrutinyModel model)
        {
            var result = await scrutinyService.Scrutiny(model);
            if (result != null)
            {
                if (result.Id == 0)
                {
                    return Ok(new { Status = false, Message = "Scrutiny Action Failed !" });
                }
                else
                    return Ok(new { Status = true, Message = result.Message });
            }
            else
            {
                return Ok(new { Status = false, Message = "Scrutiny Action Failed !" });
            }
        }
    }
}
