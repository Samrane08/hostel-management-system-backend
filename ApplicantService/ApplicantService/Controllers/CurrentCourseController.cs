using ApplicantService.Helper;
using ApplicantService.Service;
using Dapper;
using Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Model;
using MySqlX.XDevAPI.Common;
using Service.Implementation;
using Service.Interface;
using Service.Rules;
using System.ComponentModel;
using System.Data;
using System.Dynamic;

namespace ApplicantService.Controllers
{
    public class CurrentCourseController : APIBaseController
    {
    
        private readonly ICurrentCourseService currentcourseService;
        private readonly IHttpClientService httpClientService;
        private readonly IProfileService profileService;
        private readonly ILoginDetailsService loginDetailsService;

        private readonly APIUrl urloptions;

        public CurrentCourseController(ICurrentCourseService currentcourseService, IHttpClientService httpClientService, IProfileService profileService, ILoginDetailsService loginDetailsService,
        IOptions<APIUrl> urloptions)
        {
           
            this.currentcourseService = currentcourseService;
            this.httpClientService = httpClientService;
            this.urloptions = urloptions.Value;
            this.profileService = profileService;
            this.loginDetailsService = loginDetailsService;

        }
        [HttpPost("SaveCurrentQualification")]
        public async Task<IActionResult> SaveCurrentQualification(CurrentCourseDetails model)
        {
            try
            {
                //List<int> ignoreCorses = new List<int>() { 12,15,16};

               /// var courseduration =await currentcourseService.GetCourseDuration(Convert.ToInt32(model.CourseTypeID));
           
                //var personalDetails = await profileService.GetPersonalDetails();
                //if(personalDetails == null ||( personalDetails.ServiceType == 2 && Convert.ToInt32(courseduration) <2 && !ignoreCorses.Contains(model.QualificationTypeID))) {

                //    return Ok(new { status = false, message = "Course duration must be greater or equal 2 years then only you can apply for swadhar", Data = model });
                //}
                if(model.CGPA>0 && (model.CGPAFileId =="" || model.CGPAFileId==null))
                {
                    return Ok(new { status = false, message = $"Please Upload CGPA file as CGPA IS {model.CGPA}"});
                }
                else if( model.IsCompleted == true && model.Percentage <= 0)
                {
                    return Ok(new { status = false, message = "Percentage in completed year must be greater than 0.0"});
                }
                var saveResult = await currentcourseService.SaveCurrentCourseDetailsAsync(model);

                if (saveResult.Split(":")[0] != "0")
                {
                    model.Id= Convert.ToInt64(saveResult.Split(":")[0]);
                    return Ok(new { status = true, message = saveResult.Split(":")[1], Data=model });
                }
                else
                    return Ok(new { status = false, message = saveResult.Split(":")[1]});

            }
            catch (Exception ex)
            {
                return Ok(new { status = false, message = ex.Message }) ;
            }
        }
        [HttpGet("GetQualificationByuserId")]
        public async Task<object> GetQualificationByuserId()
        {
            try
            {
               
               var  result = await currentcourseService.GetCourseDetailsByUserId();
              
                if (result.Count > 0)
                    return Ok(new { status = true, message = "Data fetched successfully", CourseData = result });
                else
                    return Ok(new { status = false, message = "No data found on current filters" });


            }
            catch (Exception ex)
            {
                return Ok(new { status = false, Message = ex.Message });
            }
        }
        [HttpPost("deletecoursebyid")]
        public async Task<object> deletecoursebyid([FromBody] reqModel m)
        {
            try
            {
                var result = await currentcourseService.DeleteCurrentCourseByRowId(m.rowId);
                if(!string.IsNullOrEmpty(result))
                     return Ok(new { status = true, message = result, data = result });
                else
                    return Ok(new { status = true, message = "Some errors occured", data = result });
            }
            catch (Exception ex)
            {
                return Ok(new { status = false, message = ex.Message });
            }
        }
        //[HttpGet("IsCapRequired")]
        //public async Task<IActionResult> IsCapRequired(int courseId)
        //{
        //    try
        //    {


        //        var result = await currentcourseService.IsCapRequired(courseId);
        //        if (result != null)
        //        {

        //            return Ok(new { status = true, message = "Data got", Data = result });
        //        }
        //        else
        //            return Ok(new { status = true, message = "Data got", Data = true });
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }

        //}
        [HttpGet("CalculateYearGap")]
        public async Task<IActionResult> CalculateYearGap(int courseYr, string selectedYear)
        {
            try
            {


                var result = await currentcourseService.CalculateYearGap(courseYr, selectedYear);
                if (result != null)
                {
                    // bool Isrequired = result == "1" ? true : false;
                    return Ok(new { status = true, message = "Data got", Data = result });
                }
                else
                    return Ok(new { status = true, message = "Data got", Data = 0 });
            }
            catch (Exception ex)
            {
                return Ok(new { status = false, message = "some error occured", Data = 0 });
            }

        }
        [HttpGet("VerifyYears")]
        public async Task<IActionResult> VerifyYears(int CourseId, int YearStudy)
        {
            try
            {


                var result = await currentcourseService.CheckIsDatainYearOrder(CourseId, YearStudy);
                var verifyyeardata = CourseUtility.IsValidYearselected(result, YearStudy);
                if(!verifyyeardata.IsValidYear)
                {
                    verifyyeardata.yearsStatus = new List<Drp>();
                }

                if (verifyyeardata.IsValidYear)
                {
                    
                    return Ok(new { status = true, message = "Data got", VerifyYearData = result });
                }
                else
                    return Ok(new { status = false, message = verifyyeardata.errormsg});
            }
            catch (Exception)
            {
                return Ok(new { status = false, message = "Some error occured...." });
            }

        }
        //[HttpGet("persona_info")]
        //public async Task<IActionResult> Getpersonaldetails()
        //{
        //    try
        //    {


        //        var result = await currentcourseService.GetPersonalInfoByUserId();
        //        if (result != null)
        //        {

        //            return Ok(new { status = true, message = "Data got", Data = result });
        //        }
        //        else
        //            return Ok(new { status = false, message = "Data got", Data = "" });
        //    }
        //    catch (Exception)
        //    {
        //        return Ok(new { status = false, message = "Some error occured....", Data = "" });
        //    }

        //}
        //[HttpGet("persona_reservedCategory")]
        //public async Task<IActionResult> persona_reservedCategory()
        //{
        //    try
        //    {


        //        var result = await currentcourseService.persona_reservedCategory();
        //        if (result != null)
        //        {

        //            return Ok(new { status = true, message = "Data got", Data = result });
        //        }
        //        else
        //            return Ok(new { status = false, message = "Data got", Data = "" });
        //    }
        //    catch (Exception ex)
        //    {
        //        return Ok(new { status = false, message = "Some error occured....", Data = "" });
        //    }

        //}
        //[HttpGet("Getqualificationtype")]
        //public async Task<IActionResult> Getqualificationtype(string caller)
        //{
        //    try
        //    {


        // var result = await currentcourseService.Getqualificationtype(caller);// await Task.FromResult(dapper.GetAll<SelectListModel>("usp_GetQualificationType", null, commandType: System.Data.CommandType.StoredProcedure));
        //        return Ok(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Ok(new List<string>());
        //    }
        //}

        //[HttpGet("current-GetCourseStartYear")]
        //public async Task<IActionResult> GetCourseStartYear()
        //{
        //    try
        //    {


        //        var result = await currentcourseService.GetCurrentCourseStartYear();
        //        if (result != null)
        //        {

        //            return Ok(new { status = true, message = "Data got", Data = result });
        //        }
        //        else
        //            return Ok(new { status = true, message = "Data got", Data = 0 });
        //    }
        //    catch (Exception ex)
        //    {
        //        return Ok(new { status = false, message = "Some error occured....", Data = "" });
        //    }

        //}

        //[HttpGet("pre-school")]
        //public async Task<IActionResult> PreSchoolList(string stateId, string districtId, string talukaId)
        //{
        //    try
        //    {


        //        var result = await currentcourseService.PreSchools(stateId, districtId, talukaId);
        //        if (result != null)
        //        {

        //            return Ok(new { status = true, message = "Data got", Data = result });
        //        }
        //        else
        //            return Ok(new { status = true, message = "Data got", Data = "[]" });
        //    }
        //    catch (Exception ex)
        //    {
        //        return Ok(new { status = false, message = "Some error occured....", Data = "" });
        //    }

        //}
        //[HttpGet("pre-standard")]
        //public async Task<IActionResult> PreStandard(string caller)
        //{
        //    try
        //    {


        //        var result = await currentcourseService.PreStandard(caller);
        //        if (result != null)
        //        {

        //            return Ok(new { status = true, message = "Data got", Data = result });
        //        }
        //        else
        //            return Ok(new { status = true, message = "Data got", Data ="[]" });
        //    }
        //    catch (Exception ex)
        //    {
        //        return Ok(new { status = false, message = "Some error occured....", Data = "" });
        //    }

        //}

        //[HttpPost("SaveApplicantPreSchoolRecord")]
        //public async Task<IActionResult> SaveApplicantPreSchoolRecord(ApplicantPreSchoolRecord model)
        //{
        //    try
        //    {
        //        var saveResult = await currentcourseService.SaveApplicantPreSchoolRecord(model);

        //        if (saveResult > 0)
        //        {
        //            model.Id = saveResult;
        //            return Ok(new { status = true, message = "data saved successfully", Data = model });
        //        }
        //        else
        //            return Ok(new { status = false, message = "Some error occured", Data = model });

        //    }
        //    catch (Exception ex)
        //    {
        //        return Ok(new { status = false, message = "some error occured",Data="[]" });
        //    }
        //}
        //[HttpPost("SaveMulApplicantPreSchoolRecord")]
        //public async Task<IActionResult> SaveMulApplicantPreSchoolRecord(List<ApplicantPreSchoolRecord> model)
        //{
        //    try
        //    {
        //        var saveResult = 0;
        //        foreach (var o in model)
        //        {
        //            saveResult = await currentcourseService.SaveApplicantPreSchoolRecord(o);
        //        }

        //        if (saveResult > 0)
        //        {

        //            return Ok(new { status = true, message = "data saved successfully" });
        //        }
        //        else
        //            return Ok(new { status = false, message = "Some error occured" });

        //    }
        //    catch (Exception ex)
        //    {
        //        return Ok(new { status = false, message = "some error occured", Data = "[]" });
        //    }
        //}
        //[HttpGet("GetPreSchoolRecords")]
        //public async Task<IActionResult> GetPreSchoolRecords()
        //{
        //    try
        //    {
        //        var result = await currentcourseService.GetPreDetailsByUserId();

        //        if (result.Count > 0)
        //        {
        //            return Ok(new { status = true, message = "data got successfully", ApplicantPreSchoolRecord = result });
        //        }
        //        else
        //            return Ok(new { status = false, message = "No record found", Data = "[]" });

        //    }
        //    catch (Exception ex)
        //    {
        //        return Ok(new { status = false, message = "some error occured", Data = "[]" });
        //    }
        //}
        //[HttpPost("deletePreSchoolRecord")]
        //public async Task<IActionResult> deletePreSchoolRecord(reqModel m)
        //{
        //    try
        //    {
        //        var result = await currentcourseService.DeletePreCourseRecord(m.rowId);
        //       return Ok(new { status = true, message = "data deleted successfully", Data = result });


        //    }
        //    catch (Exception ex)
        //    {
        //        return Ok(new { status = false, message = "some error occured", Data = "[]" });
        //    }
        //}
        //[HttpGet("GetCourseStatus")]
        //public async Task<IActionResult> GetCourseStatus(string _preOrPot,int _standardId)
        //{
        //    try
        //    {
        //        var result = await currentcourseService.GetCourseStatus(_preOrPot, _standardId);
        //        return Ok(new { status = true, message = "success", Data = result });


        //    }
        //    catch (Exception ex)
        //    {
        //        return Ok(new { status = false, message = "some error occured", Data = "[]" });
        //    }
        //}

        //[HttpGet("GetCourseStatusYearByUserId")]
        //public async Task<IActionResult> GetCourseResult(string yearId)
        //{
        //    try
        //    {
        //        var result = await currentcourseService.GetCourseStatusYearByUserId(Convert.ToInt32(yearId));
        //        return Ok(new { status = true, message = "success", Data = result });


        //    }
        //    catch (Exception ex)
        //    {
        //        return Ok(new { status = false, message = "some error occured", Data = "[]" });
        //    }
        //}

        //[HttpGet("GetCurrentQualificationYear")]
        //public async Task<IActionResult> GetCurrentQualificationYear(int courseId, int? langID = 1)
        //{
        //    try
        //    {


        //        var result = await currentcourseService.GetCurrentQualificationYear(courseId, langID);
        //        return Ok(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Ok(new List<string>());
        //    }
        //}
        //[HttpGet("IsNewApplicant")]
        //public async Task<IActionResult> IsNewApplicant()
        //{
        //    try
        //    {



        //        var result = await currentcourseService.IsNewApplicant();

        //        return Ok(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Ok(new List<string>());
        //    }
        //}

        [HttpGet("CheckIsCurrentAvailable")]
        public async Task<IActionResult> CheckIsCurrentAvailable()
        {
            try
            {
                var result = await currentcourseService.CheckIsCurrentAvailable();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(0);
            }
        }


        //[HttpGet("district")]
        //public async Task<IActionResult> district(int? state, int? lang = 1)
        //{
        //    try
        //    {
        //        var result = await currentcourseService.DistList(state, lang);

        //        return Ok(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Ok(false);
        //    }
        //}

        //[HttpGet("taluka")]
        //public async Task<IActionResult> taluka(int? distid, int? lang = 1)
        //{
        //    try
        //    {
        //        var result = await currentcourseService.TalukaList(distid,lang);

        //        return Ok(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Ok(false);
        //    }
        //}

        [HttpGet("VerifyHostel")]
        public async Task<IActionResult> VerifyHostel(int district)
        {
            try
            {
                var result = await currentcourseService.GetVerifyHostel(district);
                if (!string.IsNullOrEmpty(result) && result.Split(":")[0] == "Success")
                {
                    return Ok(new { status = true, message = result.Split(":")[1]});
                }
                else
                    return Ok(new { status = false, message = result.Split(":")[1], isSwayamAadhar = result.Split(":")[2] });
            }
            catch (Exception)
            {
                return Ok(new { status = false, message = "Some error occured...." });
            }
        }

        [HttpGet("UpdateServiceType")]
        public async Task<IActionResult> UpdateServiceType(int district)
        {
            try
            {
                var result = await currentcourseService.UpdateServiceType(district);
                if (!string.IsNullOrEmpty(result) && result.Split(":")[0] == "Success")
                {
                    return Ok(new { status = true, message = result.Split(":")[1] });
                }
                else
                    return Ok(new { status = false, message = result.Split(":")[1] });
            }
            catch (Exception)
            {
                return Ok(new { status = false, message = "Some error occured...." });
            }
        }

    }
}
