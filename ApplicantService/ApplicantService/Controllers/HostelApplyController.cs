using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Model;
using Service.Interface;
using System.ComponentModel.DataAnnotations;

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
            try
            {
                var validateresult = await profileService.ValidatePreviewData();
                if (!string.IsNullOrEmpty(validateresult) && validateresult.Split(":")[0] == "Success")
                {

                    var result = await profileService.HostelApply();
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
                            if(result.ServiceType == 2)
                            {
                                return Ok(new { Status = true, Message = "Swayam Application Submitted Successfully!!", PaymentParam = result });
                            }else if (result.ServiceType == 3)
                            {
                                return Ok(new { Status = true, Message = "Aadhar Application Submitted Successfully!!", PaymentParam = result });
                            }else
                                return Ok(new { Status = true, Message = "Hostel Application Submitted Successfully!!", PaymentParam = result });
                        }
                    }
                    else
                    {
                        return Ok(new { Status = false, Message = "Hostel Apply Failed.", PaymentParam = new { } });
                    }
                }
                else
                {
                    return Ok(new { Status = false, Message = validateresult.Split(":")[1], PaymentParam = new { } });
                }
            }catch(Exception ex)
            {
                return Ok(new { Status = false, Message = "Hostel Apply Failed.", PaymentParam = new { } });
            }
        }
        [HttpPost("installment-document-final-submit")]
        public async Task<IActionResult> PostInstallmentDocFinalSubmit([FromBody] InstallmentDocFinalSubmitModel model)
        {
            var result = await profileService.InstallmentDocFinalSubmit(model);

            if (result)
            {
                return Ok(new { Status = true, FileDetails = "Attendance Submitted Successfully" });
            }
            else
                return Ok(new { Status = false, Message = "Attendance Submission Failed" });

        }
    }
}
