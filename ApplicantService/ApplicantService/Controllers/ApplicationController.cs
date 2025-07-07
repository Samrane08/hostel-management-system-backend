using Microsoft.AspNetCore.Mvc;
using Model;
using Service.Interface;

namespace ApplicantService.Controllers
{
    public class ApplicationController : APIBaseController
    {
        private readonly IProfileService profileService;
        public ApplicationController(IProfileService profileService)
        {
            this.profileService = profileService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var ApplicationList = await profileService.GetApplicationList();
            return Ok(ApplicationList);
        }
        [HttpGet("payment-status")]
        public async Task<IActionResult> PaymentStatus()
        {
            var paymentStatus = await profileService.GetPaymentParams();
            //var OfflineApplicationStatus = await profileService.GetOfflineApplication();
            if (paymentStatus != null)
            {
                if (paymentStatus._ScrutinyStatus == 5)
                {
                    return Ok(new { Status = false, ApplicatioNo = paymentStatus.AppId, ScrutinyStatus = paymentStatus._ScrutinyStatus, Message = "Application sentBack To Applicant" });
                }
                else
                {
                    if (paymentStatus.PaymentStatusId == 1)
                        return Ok(new { Status = true, ApplicatioNo = paymentStatus.AppId, ScrutinyStatus = paymentStatus._ScrutinyStatus, Message = "Application applied successfully." });
                    else if (paymentStatus.PaymentStatusId == 2)
                        return Ok(new { Status = true, ApplicatioNo = paymentStatus.AppId, ScrutinyStatus = paymentStatus._ScrutinyStatus, Message = "Application applied , kindly make  payment to complete your application." });
                    else if (paymentStatus.PaymentStatusId == 3)
                        return Ok(new { Status = true, ApplicatioNo = paymentStatus.AppId, ScrutinyStatus = paymentStatus._ScrutinyStatus, Message = "Application applied , kindly make  payment to complete your application." });
                    else if (!string.IsNullOrEmpty(paymentStatus.AppId))
                        return Ok(new { Status = true, ApplicatioNo = paymentStatus.AppId, ScrutinyStatus = paymentStatus._ScrutinyStatus, Message = "Application already generated." });
                    else
                        return Ok(new { Status = false, Message = "Application not initiated yet." });
                }
            }
            else
            {
                return Ok(new { Status = false, Message = "Application not initiated yet." });
            }
        }

        [HttpPost("cancel-application")]
        public async Task<IActionResult> CancelApplication([FromBody] string ApplicationNo)
        {
            var ApplicationList = await profileService.CancelApplication(ApplicationNo);
            return Ok(new { Status = true, Message = "Profile saved success." });
        }

        [HttpPost("accept-allocation")]
        public async Task<IActionResult> Acceptallocation([FromBody] HostelAcceptanceModel model)
        {
            var Message = await profileService.Acceptallocation(model);
            if (Message == 1)
            {
                return Ok(new { Status = true, Message = "Hostel alloted successfully." });
            }
            else if (Message == 2)
            {
                return Ok(new { Status = false, Message = "Hostel already alloted." });
            }
            else
            {
                return Ok(new { Status = false, Message = "Hostel Details incorrect." });
            }
        }

        [HttpGet("download-call-letter")]
        public async Task<IActionResult> DownloadCallLetter(int applnId)
        {
            var ApplicationList = await profileService.DownloadCallLetter(applnId);
            if (ApplicationList != null)
                return Ok(new { Status = true, Message = "Download Call letter successfully.",  Data = ApplicationList });
            else
                return Ok(new { Status = false, Message = "Download Call letter failed." });
        }


        [HttpGet("get-application-status-AtDesk")]
        public async Task<IActionResult> GetapplicationstatusAtDesk(int appln)
        {
            var ApplicationList = await profileService.GetApplicationStatusAtDesk(appln);
            if (ApplicationList != null)
                return Ok(new { Status = true, Message = "Success", Data = ApplicationList });
            else
                return Ok(new { Status = false, Message = "Failed." });
        }

        [HttpGet("get-application-installment-details")]
        public async Task<IActionResult> GetApplicationInstallmentDetails()
        {
            var InstallementName = await profileService.GetApplicationInstallmentDetails();
            if (InstallementName != null)
                return Ok(new { Status = true, Message = "Success", Data = InstallementName });
            else
                return Ok(new { Status = false, Message = "Failed." });
        }


        [HttpGet("application-status-commondesk")]
        public async Task<IActionResult> getApplicationDetailsCommon()
        {
            var ApplicationList = await profileService.GetApplicationListAtCommonDesk();
            return Ok(ApplicationList);
        }

        [HttpPost("applicant-cancel-application")]
        public async Task<IActionResult> CancelApplication([FromBody] Int64 applicationId)
        {
            var result = await profileService.CancelApplicationByApplicant(applicationId);
            if (string.IsNullOrEmpty(result))
            {
                return Ok(new { Status = false, Message = "Failed." });
            }
            if (Convert.ToInt32(result.Split(":")[0])==1)
            {
                return Ok(new { Status = true, Message = result.Split(":")[1] });
            }
             else
                return Ok(new { Status = false, Message = "Failed Please try again" });
        }
    }
}
