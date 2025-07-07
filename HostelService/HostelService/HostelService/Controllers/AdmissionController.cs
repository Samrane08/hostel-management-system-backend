using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model;
using Service.Implementation;
using Service.Interface;

namespace HostelService.Controllers
{
    
    public class AdmissionController : APIBaseController
    {
        private readonly IAdmissionService admissionService;

        public AdmissionController(IAdmissionService admissionService)
        {
                this.admissionService = admissionService;
        }

        [HttpPost("update-application-status")]
        public async Task<IActionResult> Post([FromBody] UpdateAdmissionStatusRequestModel model)
        {
            var result = await admissionService.UpdateAdmissionStatus(model);

            if(result == true)
            {
                return Ok(new { Status = true, Message = "Admission Status Updated Successfully" });
            }
            else
                return Ok(new { Status = false, Message = "Admission Status could not be updated" });

        }
    }
}
