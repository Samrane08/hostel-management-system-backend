using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model;
using Model.LotteryModel;
using Service.Implementation;
using Service.Interface;

namespace HostelService.Controllers
{
    public class DirectWardenVacancyController : APIBaseController
    {
        private readonly IDirectWardenService directWardenservice;
        private readonly IHostelProfileService hostelProfileService;
        

        public DirectWardenVacancyController(IDirectWardenService directWardenservice, IHostelProfileService hostelProfileService)
        {
            this.directWardenservice = directWardenservice;
            this.hostelProfileService = hostelProfileService;
        }

        [HttpGet("Get_Direct_warden_data")]
        public async Task<IActionResult> Get(int? HostelId)
        {
            var result = await directWardenservice.GetDirectWardenData(HostelId);
            if (result is not null)
            {
                if (result[0].msg == 1)
                    return Ok(new { Status = true, WardenVacancyData = result });
                else
                    return Ok(new { Status = false, Message = "Hostel vacancy not uploaded yet!!" });
            }
            else
            {
                return Ok(new { Message = "Data not found!" });
            }
        }

        [HttpPost("Post_Direct_warden_data")]
        public async Task<IActionResult> Post([FromBody] List<DirectWardenVacancyModel> model,int? HostelId)
        {
            var hostelProfile = new HostelProfileModel();
            var seatCalculation = model.Where(a => a.QPID == 11).FirstOrDefault();
            if(HostelId == null)
            {
                hostelProfile = await hostelProfileService.GetHostelProfile();
            }
            else
            {
                hostelProfile = await hostelProfileService.GetHostelProfileByHostelId(HostelId);
            }
            
            
            var totalseats = seatCalculation.ExistingStudent + seatCalculation.AvlSeats;

            if (totalseats != hostelProfile.Capacity)
            {
                return Ok(new { Status = false, Message = "Actual hostel capacity does not match with the sum of continuous student seats and available seats this year" });
            }

            var result = await directWardenservice.PostDirectWardenData(model, HostelId);

            if (result>0)
                return Ok(new { Status = true, Message = "Vacancy seat saved successfully" });
            else
                return Ok(new { Status = false, Message = "Unable to save warden vacancy data" });
        }
        [HttpGet("Get_priority_Quota")]
        public async Task<IActionResult> Get_priority_Quota()
        {
            var result = await directWardenservice.GetPriorityQuotaData();

            if (result is null)
                return Ok(new { Status = false, Message = "No priority data found" });
            else
                return Ok(new { Status = true, PriorityData=result});
        }
    }
}
