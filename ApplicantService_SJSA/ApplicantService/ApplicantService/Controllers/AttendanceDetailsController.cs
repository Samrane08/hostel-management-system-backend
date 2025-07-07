using ApplicantService.Service;
using Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model;
using Service.Implementation;
using Service.Interface;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ApplicantService.Controllers
{
   
    public class AttendanceDetailsController : APIBaseController
    {
        private readonly IAttendanceDetails attendanceDetailsService;
        public AttendanceDetailsController(IAttendanceDetails attendanceDetailsService)
        {
            this.attendanceDetailsService = attendanceDetailsService;
        }


        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var ApplicationList = await attendanceDetailsService.GetApplicantAttendaceDetails();
            return Ok(new { status = true,  Data = ApplicationList });
           
            //return Ok(ApplicationList);
        }
       
        [HttpPost("saveAttendanceDetails")]     
        public async Task<IActionResult> saveAttendanceDetails(AttendanceDetailsModel model)
        {
            try
            {
                //model.MaskBankAccount = model.BankAccountNo;
                var result = await attendanceDetailsService.SaveAttendanceDetailsAsync(model);
                if (!string.IsNullOrEmpty(result))
                {

                    return Ok(new { status = true, message = result });
                }
                else
                    return Ok(new { status = false, message = "Some error occured while saving bank details" });

            }
            catch (Exception ex)
            {
                return Ok(new { status = false, message = "Internal server error occured" });
            }
        }
        //test
   

    }
}
