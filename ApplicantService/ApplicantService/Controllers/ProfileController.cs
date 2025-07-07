using ApplicantService.Helper;
using ApplicantService.Service;
using Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Model;
using MySqlX.XDevAPI.Common;
using Newtonsoft.Json.Linq;
using Service.Implementation;
using Service.Interface;
using System.Text.Json;

namespace ApplicantService.Controllers
{
    public class ProfileController : APIBaseController
    {
        private readonly ILoginDetailsService loginDetailsService;
        private readonly IHttpClientService clientService;
        private readonly APIUrl urloptions;
        private readonly IProfileService profileService;

        public ProfileController(ILoginDetailsService loginDetailsService,IHttpClientService clientService, IProfileService profileService, IOptions<APIUrl> urloptions)
        {
            this.loginDetailsService = loginDetailsService;
            this.clientService = clientService;
            this.urloptions = urloptions.Value;
            this.profileService = profileService;
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
                // ExceptionLogging.LogException(Convert.ToString(ex));
                return false;
            }
        }

        [HttpGet("check-applicant-data-availble")]
        [AllowAnonymous]
        public async Task<IActionResult> CheckApplicantData(string academicYear)
        {
            var CheckApplicationData = new DbtApplicantStatusModel();
            CheckApplicationData = profileService.CheckApplicantDataAvailbility(academicYear);
            if (CheckApplicationData != null)
            {
                if (CheckApplicationData.Result == 0)
                {
                    //var applicantPrefilledData = httpClientService.RequestSend<object>(HttpMethod.Get, $"{urloptions.DbtIntegration}/HmsValidateApplicant/hms-validate-applicant-data?AadhaarReferenceNumber={1120283057393356800}&AcademicYear={academicYear}", null);


                    var httpClientService = new HttpClient();

                    //string aadhaarNumberRefNo = "1170995935935287296"; //Yash Bhai
                    string aadhaarNumberRefNo = CheckApplicationData.AadharRefNo;
                    string academicYear1 = academicYear;
                    string requestUrl = $"{urloptions.DbtIntegration}/HmsValidateApplicant/hms-validate-applicant-data?AadhaarReferenceNumber={aadhaarNumberRefNo}&AcademicYear={academicYear1}";

                    // Send GET request
                    HttpResponseMessage response = await httpClientService.GetAsync(requestUrl);
                    string jsonResponse = await response.Content.ReadAsStringAsync();

                    Console.WriteLine("Raw JSON Response:");
                    Console.WriteLine(jsonResponse);

                    if (response.IsSuccessStatusCode)
                    {
                        // Parse first-level JSON
                        JObject responseObject = JObject.Parse(jsonResponse);

                        // Extract "result" (which is a stringified JSON)
                        string resultJson = responseObject["result"]?.ToString();
                        Console.WriteLine($"Extracted Result JSON String: {resultJson}");

                        if (!string.IsNullOrEmpty(resultJson))
                        {
                            // Parse the JSON string inside "result"
                            JObject resultData = JObject.Parse(resultJson);
                            Console.WriteLine("Deserialized Result Object:");
                            Console.WriteLine(resultData.ToString());
                            var result = profileService.InsertApplicantPrefilledData(resultData.ToString(), academicYear); // give try catch for this insertion
                            if (result != null && result != -1)
                            {
                                if (result == 1)
                                {
                                    return Ok(new { Status = true, Message = "Applicant details inserted succesfully", Data = result });
                                }
                                else if (result ==0)
                                {
                                    return Ok(new { Status = false, Message = "Applicant data already availble in SJSA", Data = result });
                                }
                            }
                            else if(result == -1)
                            {
                                return Ok(new { Status = false, Message = "Something went wrong as Insertion failed", Data = result });
                            }

                        }
                        else
                        {
                            return Ok(new { Status = false, Message = "Applicant Data not availble in DBT" });
                        }
                    }

                }
                return Ok(new { Status = true, Message = "Data  Available in Autofil Table.", Data = CheckApplicationData });
            }
            else
            {
                return Ok(new { Status = false, Message = "Some Error Occured Please try after some time" });
            }
        }
    }
}