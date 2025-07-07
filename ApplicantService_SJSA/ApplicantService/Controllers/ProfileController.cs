using ApplicantService.Helper;
using ApplicantService.Service;
using Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Model;
using MySqlX.XDevAPI.Common;
using Service.Interface;
using System.Text.Json;

namespace ApplicantService.Controllers
{
    public class ProfileController : APIBaseController
    {
        private readonly ILoginDetailsService loginDetailsService;
        private readonly IHttpClientService clientService;
        private readonly APIUrl urloptions;

        public ProfileController(ILoginDetailsService loginDetailsService,IHttpClientService clientService, IOptions<APIUrl> urloptions)
        {
            this.loginDetailsService = loginDetailsService;
            this.clientService = clientService;
            this.urloptions = urloptions.Value;
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var CheckStatus = await loginDetailsService.VerifyStatus();

            if(CheckStatus != null && CheckStatus.IsAadharVerified)
            {
                var result = await loginDetailsService.GetRegisterDetails();

                if (!string.IsNullOrWhiteSpace(result.UIDNo))
                {
                    var qs = QueryStringEncryptDecrypt.DecryptQueryString(result.UIDNo);
                    string UID = qs["UID"];
                    result.UIDNo = !string.IsNullOrWhiteSpace(UID) ? "XXXXXXXX" + UID.Substring(8) : "";
                }
                return Ok(result);
            }
            else
            {
                return BadRequest();
            }     
        }

        [HttpGet("profile-image")]
        public async Task<IActionResult> GetProfileImage()
        {

            var result = await loginDetailsService.GetRegisterDetails();
            var imageResult = new FIleViewModel();
            if (result!= null && !string.IsNullOrWhiteSpace(result.ApplicantImage_string))
            {               
                try
                {
                    imageResult = await clientService.RequestSend<FIleViewModel>(HttpMethod.Get, $"{urloptions.UploadService}/File/{result.ApplicantImage_string}", null);
                }
                catch (Exception)
                {
                    imageResult = new FIleViewModel();
                }               
            }
            return Ok(imageResult);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ProfileVerifyModel model)
        {
            var resultVerification = await loginDetailsService.VerifyStatus();

            if (resultVerification != null)
            {
                if (resultVerification.IsAadharVerified == false)
                {
                    return BadRequest();
                }                
            }
            var emailexist = await loginDetailsService.CheckEmailExist(model.EmailID);
            if (emailexist)
            {
                return Ok(new { Status = false, Message = "Email exists." });
            }

            var mobileExist = await loginDetailsService.CheckMobileExist(model.MobileNo);
            if (mobileExist)
            {
                return Ok(new { Status = false, Message = "Mobile exists." });
            }            

            if (resultVerification != null && resultVerification.IsEmailVerified == false)
            {
                var IsEmailVerify = await CheckVerify($"{urloptions.NotificationService}/OTP/is-email-verified", model.EmailID);
                if (!IsEmailVerify)
                {
                    return Ok(new { Status = false, Message = "Email not verified." });
                }
            }

            if (resultVerification != null && resultVerification.IsMobileVerified == false)
            {
                var IsMobileVerify = await CheckVerify($"{urloptions.NotificationService}/OTP/is-mobile-verified", model.MobileNo);
                if (!IsMobileVerify)
                {
                    return Ok(new { Status = false, Message = "Mobile not verified." });
                }
            } 
            var result = await loginDetailsService.SaveRegisterDetails(model);
            if (result != null && result.ID != 0)
            
                return Ok(new { Status = true, Message = "Profile saved success." });
            
           else
                return Ok(new { Status = false, Message = "Profile save failed." });
        }


        [HttpPost("check-email-exists")]       
        public async Task<IActionResult> CheckEmailExists([FromBody] string email)
        {
            return Ok(await loginDetailsService.CheckEmailExist(email));
        }

        [HttpPost("check-mobile-exists")]
        public async Task<IActionResult> CheckMobileExists([FromBody] string mobile)
        {
            return Ok(await loginDetailsService.CheckMobileExist(mobile));
        }

        [HttpGet("hostel-genderwise")]
        public async Task<IActionResult> HostelGenderWise(int dist, int? taluka)
        {
            var result = await loginDetailsService.HostelGenderWise(dist, taluka);
            return Ok(result);
        }
        private async Task<bool> CheckVerify(string URL, string param)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Post, URL);
                string content = JsonSerializer.Serialize(param);
                request.Content = new StringContent(content, null, "application/json");
                HttpClient httpClient = new HttpClient();
                var response = await httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();
                var result = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<bool>(result);
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(Convert.ToString(ex));
                return false;
            }
        }
    }
}