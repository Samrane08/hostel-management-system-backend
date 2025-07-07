using Dapper;
using System.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model;
using Org.BouncyCastle.Crypto;
using Service.Interface;


namespace HostelService.Controllers
{
    public class HostelDashboardController : APIBaseController
    {
        
        private readonly IHostelDashboardService hostelDashboardService;

        public HostelDashboardController( IHostelDashboardService hostelDashboardService)
        {
            this.hostelDashboardService = hostelDashboardService;
        }

        [HttpGet("dashboard-count")]
        public async Task<IActionResult> DashboardCount(int? ServiceType, int? Installment,int? academicyear)
        {
            return Ok(await hostelDashboardService.GetHostelDashboard(ServiceType, Installment, academicyear));
        }
        [HttpGet("dashboard-ba-application-count")]
        public async Task<IActionResult> Get(int? division, int? district, int? applicationServiceType) //applicationServiceType
        {
            return Ok(await hostelDashboardService.GetBADashboardApplication(division, district, applicationServiceType));
        }



        [HttpPost("get-ba-tile-count")]
        [AllowAnonymous]
        public async Task<IActionResult> GetBATileCourseTypeCount([FromBody] ApplicationServiceType model)
        {
            try
            {
                return Ok(await hostelDashboardService.GetBATileCourseTypeDetailsCount(model));
            }
            catch (Exception ex)
            {
                return Ok(new List<SelectListModel>());
            }
        }
        [HttpGet("get-application-filters")]
        public async Task<IActionResult> GetApplicationFilters()
        {
            try
            {

                return Ok(await hostelDashboardService.GetApplicationFilters());
            }
            catch (Exception ex)
            {
                return Ok(new List<string>());
            }
        }

        //[HttpPost("hostel-applications")]
        //public async Task<IActionResult> hostelapplications([FromBody] string flag)
        //{
        //    //var result = await hostelDashboardService.GetHostelsApplications(flag);
        //    return Ok(await hostelDashboardService.GetHostelsApplications(flag));
        //    //if (result != null)
        //    //    return Ok(new { Status = true, HotelProfile = result });
        //    //else
        //    //    return Ok(new { Status = true, Message = "Hostel Profile not found." });
        //}
        [HttpGet("get-scrutiny-filter")]
        [AllowAnonymous]
        public async Task<IActionResult> GetscrutinylevelFilters(int? IsServiceType, int? IsApplicantnewExisting)
        {
            try
            {
                return Ok(await hostelDashboardService.GetscrutinylevelFilters(IsServiceType, IsApplicantnewExisting));

            }
            catch (Exception ex)
            {
                return Ok(new List<string>());
            }
        }

        [HttpPost("dashboard-ba-application-count_1")]
        [AllowAnonymous]
        public async Task<IActionResult> Post([FromBody] SearchFilter model)
        {
            try
            {
                // return Ok(await hostelDashboardService.GetBASearchFilter(model));
                return Ok(new { Status = true, Data = await hostelDashboardService.GetBASearchFilter(model) });
            }
            catch (Exception ex)
            {
                return Ok(new List<HostelDashboardModel>());
            }
        }


        [HttpPost("dashboard-ba-application-counted_data")]
        [AllowAnonymous]
        public async Task<IActionResult> GetBASearchFilterCountdata([FromBody] SearchFilter1 model)
        {
            try
            {

                return Ok(new { Status = true, Data = await hostelDashboardService.GetBASearchFilterCountdata(model) });
                //return Ok(await hostelDashboardService.GetBASearchFilterCountdata(model));
            }
            catch (Exception ex)
            {
                return Ok(new List<HostelDashboardModel>());
            }
        }


        [HttpGet("application-number")]
        [AllowAnonymous]
        public async Task<IActionResult> GetapplicationNumber(int? ayid)
        {
            try
            {
                return Ok(await hostelDashboardService.GetapplicationNumberFilter(ayid));

            }
            catch (Exception ex)
            {
                return Ok(new List<string>());
            }
        }

        // installment
        [HttpGet("get-installment-filter")]
        [AllowAnonymous]
        public async Task<IActionResult> GetInstallmentFilters(int? IsServiceType)
        {
            try
            {
                return Ok(await hostelDashboardService.GetInstallmentlevelFilters(IsServiceType));

            }
            catch (Exception ex)
            {
                return Ok(new List<string>());
            }
        }



        //[HttpPost("attendence-upload-warden")]
        //public async Task<IActionResult> WardenAttendenceUpload([FromBody] AttendenceUploadModel? model)
        //{
         
        //  //  List<AttendenceUploadModel> objmodel = new List<AttendenceUploadModel>();
        //    //objmodel = model.attenenceJsonData;
        //    Console.WriteLine("AttendenceUploadModel: " + JsonSerializer.Serialize(model.attenenceJsonData));
        //    var dataRows = model.attenenceJsonData?.Skip(1);

        //    try
        //    {
        //        if (model.attenenceJsonData.Count > 0)
        //        {
        //            var result = await hostelDashboardService.attendencr2(model);
        //            return Ok(new { Status = true, SuccessRecords = "", ErrorrRecords = "error", Message = "Records updated." });
        //        }
        //        else
        //            return Ok(new { Status = false, SuccessRecords = new List<bool>(), ErrorrRecords = new List<bool>(), Message = "No records found to update." });
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionLogging.LogException(Convert.ToString(ex));
        //        return Ok(new { Status = false, SuccessRecords = new List<bool>(), ErrorrRecords = new List<bool>(), Message = ex.Message });
        //    }
        //}


    }
}
