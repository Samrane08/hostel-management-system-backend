using Microsoft.AspNetCore.Mvc;
using Model;
using Service.Interface;

namespace NotificationService.Controllers
{
    public class EmailTemplateController : APIBaseController
    {
        private readonly IEmailTemplateService templateService;

        public EmailTemplateController(IEmailTemplateService templateService)
        {
            this.templateService = templateService;
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await templateService.GetListAsync());
        }
        [HttpGet("{key}")]
        public async Task<IActionResult> Get(string key)
        {
            return Ok(await templateService.GetByKeyAsync(key));
        }
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]EmailTemplateModel model)
        {
            await templateService.UpsertAsync(model);
            return Ok("Success");
        }
    }
}
