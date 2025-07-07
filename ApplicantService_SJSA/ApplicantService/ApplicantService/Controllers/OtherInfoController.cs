using Microsoft.AspNetCore.Mvc;
using Model;
using Service.Interface;
using Service.Rules;

namespace ApplicantService.Controllers
{
    public class OtherInfoController : APIBaseController
    {
        private readonly IProfileService profileService;

        public OtherInfoController(IProfileService profileService)
        {
            this.profileService = profileService;
        }
        [HttpGet]
        public async Task<IActionResult> get()
        {
            var result = await profileService.GetOtherDetails();
            return Ok(new { Status = true, OtherDetails = result });
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] OtherDetailsModel model)
        {
            var personalDetails = await profileService.GetPersonalDetails();

            var percentage = await profileService.GetMostRecentCoursepercentage();

            if (personalDetails != null && personalDetails.ServiceType == 2 && personalDetails.IsNewApplicant)
            {
                var validateresponse = SwadharBussinessRules.ValidatePastEducationDetails(personalDetails, model, percentage);
                if (!string.IsNullOrEmpty(validateresponse))
                    return Ok(new { Status = false, Message = validateresponse });

            }
            var paymentStatus = await profileService.GetPaymentParams();
            if (paymentStatus != null)
            {
                if (!string.IsNullOrEmpty(paymentStatus.AppId) && paymentStatus._ScrutinyStatus != 5)
                    return Ok(new { Status = false, Message = "Application already applied.No changes allowed." });
            }

            var result = await profileService.SaveOtherDetails(model);
            if (result.UserId != 0)
                return Ok(new { Status = true, Message = "Other Details saved success." });
            else
                return Ok(new { Status = false, Message = "Other Details save failed." });

        }
        //public async Task<string>ValidatePastEducationDetails()
        //{
        //    string errorMessage = "";

        //    var personalDetails = await profileService.GetPersonalDetails();
        //    var otherDetails = await profileService.GetOtherDetails();
        //    var percentage = await profileService.GetMostRecentCoursepercentage();
        //    if (personalDetails != null && personalDetails.IsNewApplicant && personalDetails.ServiceType == 2)
        //    {
        //        if (personalDetails != null && otherDetails != null && percentage > 0)
        //        {
        //            if (Convert.ToBoolean(otherDetails.IsApplicantDisable) && percentage < 40)
        //            {
        //                errorMessage= "To proceed, you must have a minimum percentage of 40%. Please adjust your input and try again.";

        //            }
        //            else if (!Convert.ToBoolean(otherDetails.IsApplicantDisable) && percentage < 50)
        //            {
        //                errorMessage= "To proceed, you must have a minimum percentage of 50%. Please adjust your input and try again.";

        //            }
        //        }
        //        else
        //        {
        //            errorMessage= "Please fill the personal details ,current course and past education , and past course percentage can not be 0";
        //        }
        //    }else
        //    {
        //        errorMessage = "";
        //    }
        //    return errorMessage;
        //}
    }
}
