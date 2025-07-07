using Helper;
using HostelService.Helper;
using HostelService.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Model;
using Service.Interface;
using System.Text.Json;

namespace HostelService.Controllers
{ 
    public class AdmittedApplicantController : APIBaseController
    {
        private readonly IAdmittedRegistrationService AdmittedRegistrationService;
        private readonly IHttpClientService httpClientService;
        private readonly APIUrl urloptions;

        public AdmittedApplicantController(IAdmittedRegistrationService AdmittedRegistrationService,
                                             IHttpClientService httpClientService,
                                             IOptions<APIUrl> urloptions)
        {
            this.AdmittedRegistrationService = AdmittedRegistrationService;
            this.httpClientService = httpClientService;
            this.urloptions = urloptions.Value;
        }
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] AdmittedRegistrationModel model)
        {
            List<AdmittedAadhharList> success = new List<AdmittedAadhharList>();
            List<AdmittedAadhharList> error = new List<AdmittedAadhharList>();
            List<AdmittedAadhharList> objmodel = new List<AdmittedAadhharList>();
            objmodel = model.aadhaarData;

            try
            {
                if (objmodel.Count > 0)
                {
                    foreach (var item in objmodel)
                    {
                        if (AdmittedRegistrationService.IsRecordValidate(item))
                        {
                            item.UIDNoref = "0";
                            item.EncryptedAadhar = QueryStringEncryptDecrypt.EncryptQueryString("UID=" + item.UIDNo);
                            success.Add(item);
                        }
                        else
                        {
                            item.UIDNoref = "Invalid UID Number";
                            error.Add(item);
                        }
                    }
                    await AdmittedRegistrationService.Registration(JsonSerializer.Serialize(success), model.courseType);
                    return Ok(new { Status = true, SuccessRecords = success, ErrorrRecords = error, Message = "Records updated." });
                }
                else
                    return Ok(new { Status = false, SuccessRecords = new List<bool>(), ErrorrRecords = new List<bool>(), Message = "No records found to update." });
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(Convert.ToString(ex));
                return Ok(new { Status = false, SuccessRecords = new List<bool>(), ErrorrRecords = new List<bool>(), Message = ex.Message });
            }
        }
    }
}
