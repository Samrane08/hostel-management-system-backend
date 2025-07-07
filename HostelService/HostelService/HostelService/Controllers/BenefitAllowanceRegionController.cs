using Microsoft.AspNetCore.Mvc;
using Model;
using Service.Interface;

namespace HostelService.Controllers
{
    public class BenefitAllowanceRegionController : APIBaseController
    {
        private readonly IBenefitAllowanceService benefitAllowanceService;

        public BenefitAllowanceRegionController(IBenefitAllowanceService benefitAllowanceService)
        {
            this.benefitAllowanceService = benefitAllowanceService;
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await benefitAllowanceService.GetListAsync());
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] BenefitAloowanceCategoryInsertModel model)
        {
            var data = await benefitAllowanceService.UsertAsync(model);
            if (data)
                return Ok(new { Status = true, Message = "Benefit Allowance Region Update Successfully." });
            else 
                return Ok(new { Status = false, Message = "Benefit Allowance Region Update Failed." });
        }
    }
}
