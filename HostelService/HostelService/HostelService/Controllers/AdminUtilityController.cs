using Helper;
using HostelService.Helper;
using HostelService.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Model;
using Service.Interface;
using System.Data;
using System.Net;

namespace HostelService.Controllers
{

    public class AdminUtilityController : APIBaseController
    {
        private readonly IAdminUtilityService adminUtilityService;
        private readonly IHttpClientService httpClientService;
        private readonly APIUrl urloptions;

        public AdminUtilityController(IAdminUtilityService adminUtilityService, IHttpClientService httpClientService, IOptions<APIUrl> urloptions)
        {
            this.adminUtilityService = adminUtilityService;
            this.httpClientService = httpClientService;
            this.urloptions = urloptions.Value;

        }
        [HttpGet("get-payment-status")]
        public async Task<IActionResult> GetPaymentStatus(string applicationNo)
        {
            var response = await adminUtilityService.GetPaymentStatusAsync(applicationNo);

            if (response != null)
            {
                return Ok(new { Status = true, Message = response });
            }
            else
                return Ok(new { Status = false, Message = "Payment Status not available" });

        }
        [HttpGet("verify-super-admin-mobile")]
        public async Task<IActionResult> VerifySuperAdminMobile(string MobileNo)
        {
            var response = await adminUtilityService.VerifySuperAdminMobile(MobileNo);

            if (response != null && response == true)
            {
                return Ok(await httpClientService.RequestSend<object>(HttpMethod.Post, $"{urloptions.NotificationService}/OTP/SMS/Send", MobileNo));
            }
            else
                return Ok(new { HttpStatusCode = HttpStatusCode.OK, Data = new { Status = false, Message = "Invalid Mobile No" } });

        }
        [HttpGet("get-application-id")]
        public async Task<IActionResult> GetApplicationId(string applicationNo)
        {
            var response = await adminUtilityService.GetApplicationIdAsync(applicationNo);
            if (response != null)
            {
                return Ok(new { Status = true, Message = response });
            }
            else
                return Ok(new { Status = false, Message = "Application Number not available" });
        }

        [HttpGet("get-application-by-Aadhar")]
        public async Task<IActionResult> GetApplicationDetailByAadhar(string aadharNo)
        {
            try
            {
                var decryptedAadhaar = AESDecryption.DecryptString(aadharNo);
                var aadhar_wo = decryptedAadhaar.Substring(5, decryptedAadhaar.Length - 10);
                //var aadharRefNo = await httpClientService.RequestSend<object>(HttpMethod.Post, $"{urloptions.AadharService}/Aadhaar/get-reference",aadharNo);
                var aadharRefNo = QueryStringEncryptDecrypt.EncryptQueryString("UID=" + aadhar_wo);
                if (!string.IsNullOrEmpty(aadharRefNo))
                {
                    var response = await adminUtilityService.GetApplicationByAadharRef(aadharRefNo);
                    if (response != null)
                    {
                        return Ok(new { Status = true, UserDetail = response });
                    }
                    else
                        return Ok(new { Status = false, Message = "Aadhar Number Does Not Exist" });
                }
                else
                    return Ok(new { Status = false, Message = "Aadhar Number Does Not Exist" });

            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(Convert.ToString(ex));
                return BadRequest(new { Status = false, Message = "Inrnel Server Error" });
            }

        }
         
        [HttpGet("get-application-by-Email")]
        public async Task<IActionResult> GetApplicationDetailByEmail(string email)
        {
            try
            {
                //var aadharRefNo = await httpClientService.RequestSend<object>(HttpMethod.Post, $"{urloptions.AadharService}/Aadhaar/get-reference",aadharNo);
                if (!string.IsNullOrEmpty(email))
                {
                    var response = await adminUtilityService.GetApplicationByEmail(email);
                    if (response != null)
                    {
                        return Ok(new { Status = true, UserDetail = response });
                    }
                    else
                        return Ok(new { Status = false, Message = "Email Does Not Exist" });
                }
                else
                    return Ok(new { Status = false, Message = "Email Does Not Exist" });

            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(Convert.ToString(ex));
                return BadRequest(new { Status = false, Message = "Inrnel Server Error" });
            }

        }
        [HttpGet("get-application-by-Mobile")]
        public async Task<IActionResult> GetApplicationDetailByMobile(string mobile)
        {
            try
            {
                //var aadharRefNo = await httpClientService.RequestSend<object>(HttpMethod.Post, $"{urloptions.AadharService}/Aadhaar/get-reference",aadharNo);
                if (!string.IsNullOrEmpty(mobile))
                {
                    var response = await adminUtilityService.GetApplicationByMobile(mobile);
                    if (response != null)
                    {
                        return Ok(new { Status = true, UserDetail = response });
                    }
                    else
                        return Ok(new { Status = false, Message = "Mobile Does Not Exist" });
                }
                else
                    return Ok(new { Status = false, Message = "Mobile Does Not Exist" });

            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(Convert.ToString(ex));
                return BadRequest(new { Status = false, Message = "Inrnel Server Error" });
            }

        }

        [HttpGet("sync-dbt-course-data")]
        [AllowAnonymous]
        public async Task<IActionResult> syncDbtCourseData()
        {

            var response = await httpClientService.RequestSend<object>(HttpMethod.Get, $"{urloptions.DbtIntegration}/HMSCourseCollageUpdation/dbt-course-updation", null);

            if (response != null)
            {
                return Ok(new { Status = true, Message = response });
            }
            else
                return Ok(new { Status = false, Message = "Course sync failed" });

        }

        //[HttpGet("get-ay-application-details")]
        //public async Task<IActionResult> GetAyApplicationDetails()
        //{
        //    var response = await adminUtilityService.GetAyApplicationDetails();

        //    if (response != null)
        //    {
        //        return Ok(new { Status = true, Message = "Data save successful!!" });
        //    }
        //    else
        //        return Ok(new { Status = false, Message = "Failed!!" });

        //}

        [HttpGet("sync-dbt-college-data")]
        [AllowAnonymous]
        public async Task<IActionResult> syncDbtCollegeData()
        {
            //var applicantPrefilledData = httpClientService.RequestSend<string>(HttpMethod.Get, $"{urloptions.DbtIntegration}/HmsValidateApplicant/hms-validate-applicant-data?AadhaarReferenceNumber", null);

            var response = await httpClientService.RequestSend<object>(HttpMethod.Get, $"{urloptions.DbtIntegration}/HMSCourseCollageUpdation/dbt-college-updation", null);

            if (response != null)
            {
                return Ok(new { Status = true, Message = response });
            }
            else
                return Ok(new { Status = false, Message = "College sync failed" });

        }

        [HttpGet("sync-dbt-college-course-mapping-updation-data")]
        [AllowAnonymous]
        public async Task<IActionResult> syncDbtCourseCollegeData()
        {
            //var applicantPrefilledData = httpClientService.RequestSend<string>(HttpMethod.Get, $"{urloptions.DbtIntegration}/HmsValidateApplicant/hms-validate-applicant-data?AadhaarReferenceNumber", null);

            var response = await httpClientService.RequestSend<object>(HttpMethod.Get, $"{urloptions.DbtIntegration}/HMSCourseCollageUpdation/dbt-college-course-mapping-updation", null);

            if (response != null)
            {
                return Ok(new { Status = true, Message = response });
            }
            else
                return Ok(new { Status = false, Message = "College-Course-mapping sync failed" });

        }

        [HttpPost("update-closing-date")]
        [AllowAnonymous]
        public async Task<IActionResult> updateexpiredate([FromBody] ClosingDateModel model)
        {
            try
            {
                return Ok(new { status = true, Message = "Date Updated Successfully", Data = await adminUtilityService.updateClosingDate(model) });

            }
            catch (Exception ex)
            {
                return Ok(new List<HostelDashboardModel>());
            }
        }


        [HttpGet("get-servicetype-dropdown")]
        [AllowAnonymous]
        public async Task<IActionResult> ClosingDateServiceType()
        {
            return Ok(await adminUtilityService.GetServiceTypeClosingDate());
        }

        [HttpGet("get-update-closing-date")]
        [AllowAnonymous]
        public async Task<IActionResult> GetupdateExpiredate(string DeptId)

        {
            return Ok(await adminUtilityService.GetUpdateClosingDate(DeptId));
            //

        }


        [HttpGet("get-parentid-menumapping-dropdown")]
        [AllowAnonymous]
        public async Task<IActionResult> GetParentIdMenuMapping()
        {
            return Ok(await adminUtilityService.GetparentIdAddMenuMapping());
        }

        [HttpPost("insert-menu-btn")]
        [AllowAnonymous]
        public async Task<IActionResult> Post([FromBody] MenuInsertModel model)
        {
            try
            {
                var result = await adminUtilityService.SaveMenubtnMapping(model);
                if (!string.IsNullOrEmpty(result) && result.Split(":")[0] == "Success")
                    //  return Ok(new { Status = true, Message = result.Split(":")[1] });
                    return Ok(new { Status = true, Message = result });

                else
                    return Ok(new { Status = false, Message = "Menu saved failed." });
            }
            catch (Exception ex)
            {
                return Ok(new { Status = false, Message = "Menu save failed." });
            }

        }


        [HttpGet("get-rolelist-dept-dropdown")]
        [AllowAnonymous]
        public async Task<IActionResult> rolelistdeptid(string departmentid)
        {
            var response = await adminUtilityService.GetRoleListDeptid(departmentid);
            if (response != null)
            {
                return Ok(new { Status = true, Message = response });
            }
            else
                return Ok(new { Status = false, Message = "role sync failed" });
        }


        [HttpGet("get-menuList-dropdown")]
        [AllowAnonymous]
        public async Task<IActionResult> GetMenuList()
        {
            return Ok(await adminUtilityService.GetMenuListAll());
        }



        [HttpPost("insert-menumapping-btn")]
        [AllowAnonymous]
        public async Task<IActionResult> insertmenumapping([FromBody] MenuMappingInsert model)
        {
            try
            {
                var result = await adminUtilityService.SaveMenuMapping(model);
                if (!string.IsNullOrEmpty(result) && result.Split(":")[0] == "Success")
                    return Ok(new { Status = true, Message = result.Split(":")[1] });
                else
                    return Ok(new { Status = false, Message = "Menu saved failed." });
            }
            catch (Exception ex)
            {
                return Ok(new { Status = false, Message = "Menu save failed." });
            }

        }



    }
}
