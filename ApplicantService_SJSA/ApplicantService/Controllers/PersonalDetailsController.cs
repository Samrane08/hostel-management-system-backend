using Google.Protobuf.WellKnownTypes;
using Helper;
using Microsoft.AspNetCore.Mvc;
using Model;
using Service.Interface;

namespace ApplicantService.Controllers;

public class PersonalDetailsController : APIBaseController
{
    private readonly IProfileService profileService;
    private readonly ILoginDetailsService loginDetailsService;

    public PersonalDetailsController(IProfileService profileService, ILoginDetailsService loginDetailsService)
    {
        this.profileService = profileService;
        this.loginDetailsService = loginDetailsService;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var result = await profileService.GetPersonalDetails();
        return Ok(new { Status = true, PersonalDetails = result });
    }
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] PersonalDetailsModel model)
    {
        //var ServiceDateCheck = await loginDetailsService.VerifyStatus();
        try
        {

            if (model.ServiceType == null || model.ServiceType <= 0)
            {
                return Ok(new { Status = false, Message = "Invalid service type" });
            }
            bool IsApplicationExist = await profileService.CheckApplicationAlreadyExistForService(model.ServiceType);
            if (IsApplicationExist)
            {
                return Ok(new { Status = false, Message = "You can't apply multiple multiple application for one service,application already exist." });
            }

            var paymentStatus = await profileService.GetPaymentParams();
            if (paymentStatus != null)
            {
                if (!string.IsNullOrEmpty(paymentStatus.AppId) && paymentStatus._ScrutinyStatus != 5)
                {
                    return Ok(new { Status = false, Message = "Application already applied.No changes allowed." });
                }
            }

            if (model.IsMaharastraDomicile == null || model.IsMaharastraDomicile == false)
                return Ok(new { Status = false, Message = "Only Maharashtra domicile applicants are eligible." });

            if (model.ServiceType == 2) // Only allow for SC Caste
            {
                if (model.TypeOfCourse == 5)

                    return Ok(new { Status = false, Message = "Secondary course type is not allowed to apply swadhar." });

                if (model.CasteCategory != 2)
                    return Ok(new { Status = false, Message = "Only SC applicants are allowed to apply swadhar." });
            }

            var OfflineApplicationStatus = await profileService.GetOfflineApplication();
            if (OfflineApplicationStatus != null)
            {
                if (OfflineApplicationStatus.IsOfflineApplication == 1 && model.ServiceType == 1 && !model.IsNewApplicant)
                {
                    return Ok(new { Status = false, Message = "As your application is offline. You have to select New hostel." });
                }
                if (OfflineApplicationStatus.IsOfflineApplication == 1 && model.ServiceType == 2)
                {
                    return Ok(new { Status = false, Message = "As your application is offline. You have to select New hostel." });
                }
                //if (OfflineApplicationStatus.IsOfflineApplication == 1 && model.TypeOfCourse != OfflineApplicationStatus.typeOfCourse)
                //{
                //    return Ok(new { Status = false, Message = "As your application is offline. Selected Type of course in personal details not as per the offline data uploaded by Warden." });
                //}
            }

            var AdmittedApplicationStatus = await profileService.GetAdmittedApplication();
            if (AdmittedApplicationStatus != null)
            {
                if (AdmittedApplicationStatus.IsAdmittedApplication == 1 && model.ServiceType == 1 && !model.IsNewApplicant)
                {
                    return Ok(new { Status = false, Message = "As your application is offline. You have to select New hostel in Personal Details." });
                }
                if (AdmittedApplicationStatus.IsAdmittedApplication == 1 && model.ServiceType == 2)
                {
                    return Ok(new { Status = false, Message = "As your application is offline. You have to select New hostel in Personal Details." });
                }
                //if (AdmittedApplicationStatus.IsAdmittedApplication == 1 && model.TypeOfCourse != AdmittedApplicationStatus.typeOfCourse)
                //{
                //    return Ok(new { Status = false, Message = "As your application is offline. Selected Type of course in personal details not as per the offline data uploaded by Warden." });
                //}
            }

            var result = await profileService.SavePersonalDetails(model);
            if (!string.IsNullOrWhiteSpace(result.ApplicantName))

                return Ok(new { Status = true, Message = "Personal details saved successfully." });

            else
                return Ok(new { Status = false, Message = "Personal details save failed." });
        }catch(Exception ex)
        {
            ExceptionLogging.LogException(Convert.ToString(ex));
            return Ok(new { Status = false, Message = "Internal server error." });
        }
    }
}
