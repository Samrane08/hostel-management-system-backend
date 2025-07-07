using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Service.Interface;

namespace ApplicantService.Controllers
{
    public class HostelApplyController : APIBaseController
    {
        private readonly IProfileService profileService;
        private readonly ILoginDetailsService loginDetailsService;

        public HostelApplyController(IProfileService profileService, ILoginDetailsService loginDetailsService)
        {
            this.profileService = profileService;
            this.loginDetailsService = loginDetailsService;
        }

        [HttpPost]
        public async Task<IActionResult> Post()
        {
            var personalDetails = await profileService.GetPersonalDetails();
            var ServiceType = personalDetails.ServiceType;

            //var CheckServiceEnable = await loginDetailsService.VerifyStatus();
            //if (ServiceType == 1 && CheckServiceEnable.HostelEnable == false)
            //{
            //    return Ok(new { Status = false, Message = "Hostel apply date has been expired." });
            //}

            if (ServiceType == 2 && personalDetails.CasteCategory != 2)
            {
                return Ok(new { Status = false, Message = "Only SC applicants are allowed to apply swadhar." });
            }else if(ServiceType == 2 && personalDetails.CasteCategory ==2)
            {
                string respmsg = await profileService.CheckValidationForSwadharApply();
                if(respmsg != null && respmsg.Split(":").Length>0)
                {
                    if (respmsg.Split(":")[0]=="0")
                    {
                        return Ok(new { Status = false, Message = respmsg.Split(":")[1] });
                    }

                }
                else
                {
                    return Ok(new { Status = false, Message = "Unable to verify swadhar criteria." });

                }
                
            }

            var OfflineApplicationStatus = await profileService.GetOfflineApplication();
            if (OfflineApplicationStatus != null)
            {
                if (OfflineApplicationStatus.IsOfflineApplication == 1 && personalDetails.ServiceType == 1 && !personalDetails.IsNewApplicant)
                {
                    return Ok(new { Status = false, Message = "As your application is offline. You have to select New hostel in Personal Details." });
                }
                if (OfflineApplicationStatus.IsOfflineApplication == 1 && personalDetails.ServiceType == 2)
                {
                    return Ok(new { Status = false, Message = "As your application is offline. You have to select New hostel in Personal Details." });
                }
                //if (OfflineApplicationStatus.IsOfflineApplication == 1 && personalDetails.TypeOfCourse != OfflineApplicationStatus.typeOfCourse)
                //{
                //    return Ok(new { Status = false, Message = "As your application is offline. Selected Type of course in personal details not as per the offline data uploaded by Warden." });
                //}
            }

            var AdmittedApplicationStatus = await profileService.GetAdmittedApplication();
            if (AdmittedApplicationStatus != null)
            {
                if (AdmittedApplicationStatus.IsAdmittedApplication == 1 && personalDetails.ServiceType == 1 && !personalDetails.IsNewApplicant)
                {
                    return Ok(new { Status = false, Message = "As your application is Admitted for New Hostel Professional Course. You have to select New hostel in Personal Details." });
                }
                if (AdmittedApplicationStatus.IsAdmittedApplication == 1 && personalDetails.ServiceType == 2)
                {
                    return Ok(new { Status = false, Message = "As your application is Admitted for New Hostel Professional Course. You have to select New hostel in Personal Details." });
                }
                //if (AdmittedApplicationStatus.IsAdmittedApplication == 1 && personalDetails.TypeOfCourse != AdmittedApplicationStatus.typeOfCourse)
                //{
                //    return Ok(new { Status = false, Message = "As your application is offline. Selected Type of course in personal details not as per the offline data uploaded by Warden." });
                //}
            }
       


            var paymentStatus = await profileService.GetPaymentParams();
            if (paymentStatus != null)
            {
                if (!string.IsNullOrEmpty(paymentStatus.AppId) && paymentStatus._ScrutinyStatus != 5)
                    return Ok(new { Status = false, Message = "Application already genereated." });
            }
            var validate = await profileService.ValidateProfile();
            if (validate.PersonalProfile != 1)
            {
                return Ok(new { Status = false, Message = "Personal profile not updated." });
            }
            if (validate.ApplicantAddress != 1)
            {
                return Ok(new { Status = false, Message = "Address is not updated." });
            }
            if (validate.ParentAddress != 1)
            {
                return Ok(new { Status = false, Message = "Parent address is not updated." });
            }
            if (validate.CurrentQualification != 1)
            {
                return Ok(new { Status = false, Message = "Pursuing education required to update in current qualification." });
            }
            if (validate.PastQualification != 1)
            {
                return Ok(new { Status = false, Message = "Past qualification is not updated." });
            }
            if (validate.OtherDetails != 1)
            {
                return Ok(new { Status = false, Message = "Other details is not updated." });
            }
            if (ServiceType == 1 && validate.HostelPreference != 1)
            {
                return Ok(new { Status = false, Message = "Hostel preference not set." });
            }

            if (ServiceType == 1)
            {
                var hostels = await profileService.GetHostelPreference(1);
                if (hostels.IsNullOrEmpty())
                {
                    return Ok(new { Status = false, Message = "Hostel preference not set." });
                }
                if(hostels.Select(x=>x.Preference).FirstOrDefault() == 0)
                {
                    return Ok(new { Status = false, Message = "Hostel preference not set." });
                }
            }

            if (validate.DocumentUpoad != 1)
            {
                return Ok(new { Status = false, Message = "Documents are not uploaded." });
            }

            var result = await profileService.HostelApply(validate.ApplicationId);
            if (result != null)
            {
                if (result.Id != 0)
                {
                    await profileService.ApplicationBackup(result.Id);
                }
                if (result.ProceedToPay != 0)
                {
                    return Ok(new { Status = true, Message = "Details are completed,Moving to Payment Section", PaymentParam = result });
                }
                else
                {
                    return Ok(new { Status = true, Message = "Hostel Apply successfully!!", PaymentParam = result });
                }
            }
            else
            {
                return Ok(new { Status = false, Message = "Hostel apply failed.", PaymentParam = new { } });
            }
        }
    }
}
