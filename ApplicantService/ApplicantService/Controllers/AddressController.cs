using ApplicantService.Helper;
using ApplicantService.Service;
using Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Model;
using Service.Interface;

namespace ApplicantService.Controllers
{

    public class AddressController : APIBaseController
    {
        private readonly IProfileService profileService;

        private readonly IHttpClientService httpClientService;
        private readonly ILoginDetailsService loginDetailsService;

        private readonly APIUrl urloptions;
        public AddressController(IProfileService profileService, IHttpClientService httpClientService,ILoginDetailsService loginDetailsService,
        IOptions<APIUrl> urloptions)
        {
            this.profileService = profileService;
            this.httpClientService = httpClientService;
            this.urloptions = urloptions.Value;
            this.loginDetailsService = loginDetailsService;
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var data = await profileService.GetAddressDetails(0);           
            return Ok(new { Status = true , Address = data });
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] AddressDetailsModel model)
        {
            //var paymentStatus = await profileService.GetPaymentParams();
            //if (paymentStatus != null)
            //{
            //    if (!string.IsNullOrEmpty(paymentStatus.AppId) && paymentStatus._ScrutinyStatus != 5)
            //        return Ok(new { Status = false, Message = "Application already applied.No changes allowed." });
            //}
            try
            {
                var result = await profileService.SaveAddressDetails(model);
                if (!string.IsNullOrEmpty(result) && result.Split(":")[0] == "Success")
                    return Ok(new { Status = true, Message = result.Split(":")[1] });
                else
                    return Ok(new { Status = false, Message = "Address save failed." });
            }catch (Exception ex)
            {
                return Ok(new { Status = false, Message = "Address save failed." });
            }
        }
        [HttpGet("parent-address")]
        public async Task<IActionResult> GetParentAddress()
        {
           
           var data = await profileService.GetParentAddressDetails(1);            
            return Ok(new { Status = true, ParentAddress = data });
        }
        [HttpPost("parent-address")]
        public async Task<IActionResult> SaveParentAddress([FromBody]ParentAddressModel model)
        {
            //var paymentStatus = await profileService.GetPaymentParams();
            //if (paymentStatus != null)
            //{
            //    if (!string.IsNullOrEmpty(paymentStatus.AppId) && paymentStatus._ScrutinyStatus != 5)
            //        return Ok(new { Status = false, Message = "Application already applied.No changes allowed." });
            //}
            try
            {
                var result = await profileService.SaveParentAddressDetails(model);
                if (!string.IsNullOrEmpty(result) && result.Split(":")[0] == "Success")
                    return Ok(new { Status = true, Message = result.Split(":")[1] });
                else
                    return Ok(new { Status = false, Message = "Parent Address save failed." });
            }catch(Exception ex) {
                return Ok(new { Status = false, Message = "Parent Address save failed." });
            }
        }

    }
}
