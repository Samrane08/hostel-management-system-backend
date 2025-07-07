using Helper;
using HostelService.Helper;
using HostelService.Model;
using HostelService.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Model;
using Repository.Interface;
using Service.Interface;
using System.ComponentModel;
using System.Data;
using System.Dynamic;
using System.Net;
using System.Reflection;
using System.Security.Cryptography.Xml;

namespace HostelService.Controllers
{
    public class ApplicationController : APIBaseController
    {
        private readonly IApplicationService applicationService;
        private readonly IHttpClientService clientService;
        private readonly ICurrentUserService currentUserService;
        private readonly APIUrl urloptions;
        public ApplicationController(IApplicationService applicationService,
                                     IHttpClientService clientService,
                                     IOptions<APIUrl> urloptions,ICurrentUserService currentUserService)
        {
            this.applicationService = applicationService;
            this.clientService = clientService;
            this.currentUserService = currentUserService;
            this.urloptions = urloptions.Value;
        }
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] SearchApplicationModel model)
        {
            return Ok(await applicationService.GetListAsync(model));
        }

        [HttpPost("search-offline-scrutiny")]
        public async Task<IActionResult> Post([FromBody] SearchApplicationOfflineModel model)
        {
            return Ok(await applicationService.GetListOfflineAsync(model));
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> Get(long Id)
        {
            var result = await applicationService.GetByIdAsync(Id);
            dynamic obj = new ExpandoObject();

            if (result != null)
            {
                if (result.AadharData != null)
                {
                    var imageResult = new FIleViewModel();
                    if (!string.IsNullOrWhiteSpace(result.AadharData.AadhaarImage))
                    {
                        try
                        {
                            imageResult = await clientService.RequestSend<FIleViewModel>(HttpMethod.Get, $"{urloptions.UploadService}/File/{result.AadharData.AadhaarImage}", null);
                        }
                        catch (Exception)
                        {
                            imageResult = new FIleViewModel();
                        }
                        obj.AadhaarImage = imageResult;
                    }
                    if (!string.IsNullOrWhiteSpace(result.AadharData.UIDNo))
                    {
                        var qs = QueryStringEncryptDecrypt.DecryptQueryString(result.AadharData.UIDNo);
                        string UID = qs["UID"];
                        result.AadharData.UIDNo = !string.IsNullOrWhiteSpace(UID) ? "XXXXXXXX" + UID.Substring(8) : "";
                    }
                    var formattedAadharResult = result.AadharData.ToType<AadhaarPreviewResponseModel>();
                    obj.AadharDetails = ConvertToKeyValuePairs(formattedAadharResult);
                }
                else
                {
                    obj.AadhaarImage = new FIleViewModel();
                    obj.AadharDetails = new List<string>();
                }

                if (result.PersonalDetails != null)
                    obj.PersonalDetails = ConvertToKeyValuePairs(result.PersonalDetails);
                else
                    obj.PersonalDetails = new List<string>();

                if (result.CastDetails != null)

                    obj.CastDetails = ConvertToKeyValuePairs(result.CastDetails);
                else
                    obj.CastDetails = new List<string>();

                if (result.DomicileDetails != null)

                    obj.DomicileDetails = ConvertToKeyValuePairs(result.DomicileDetails);
                else
                    obj.DomicileDetails = new List<string>();

                if (result.PermanentAddress != null)
                    obj.PermanentAddress = ConvertToKeyValuePairs(result.PermanentAddress);
                else
                    obj.PermanentAddress = new List<string>();

                if (result.PresentAddress != null)
                    obj.PresentAddress = ConvertToKeyValuePairs(result.PresentAddress);
                else
                    obj.PresentAddress = new List<string>();

                if (result.ParentAddress != null)
                    obj.ParentAddress = ConvertToKeyValuePairs(result.ParentAddress);
                else
                    obj.ParentAddress = new List<string>();

                if (result.GuardianAddress != null)
                    obj.GuardianAddress = ConvertToKeyValuePairs(result.GuardianAddress);
                else
                    obj.GuardianAddress = new List<string>();

                if (result.OtherDetails != null)
                    obj.OtherDetails = ConvertToKeyValuePairs(result.OtherDetails);
                else
                    obj.OtherDetails = new List<string>();

                obj.CurrentCourse = new List<string>();

                if (result.CurrentCourse != null)
                {
                    List<List<Dictionary<string, object>>> currentCourseList = new List<List<Dictionary<string, object>>>();
                    foreach (var item in result.CurrentCourse)
                    {
                        currentCourseList.Add(ConvertToKeyValuePairs(item));
                    }
                    obj.CurrentCourse = currentCourseList;
                }

                if (result.PreCurrentCourse != null && result.PreCurrentCourse.Count > 0)
                {
                    List<List<Dictionary<string, object>>> currentCourseList = new List<List<Dictionary<string, object>>>();
                    foreach (var item in result.PreCurrentCourse)
                    {
                        currentCourseList.Add(ConvertToKeyValuePairs(item));
                    }
                    obj.CurrentCourse = currentCourseList;
                }

                obj.PastQualification = new List<string>();

                if (result.PastCourse != null && result.PastCourse.Count > 0)
                {
                    List<List<Dictionary<string, object>>> pastCourseList = new List<List<Dictionary<string, object>>>();
                    foreach (var item in result.PastCourse)
                    {
                        pastCourseList.Add(ConvertToKeyValuePairs(item));
                    }
                    obj.PastQualification = pastCourseList;
                }
                else if (result.PrePastCourse != null && result.PrePastCourse.Count > 0)
                {
                    List<List<Dictionary<string, object>>> pastCourseList = new List<List<Dictionary<string, object>>>();
                    foreach (var item in result.PrePastCourse)
                    {
                        pastCourseList.Add(ConvertToKeyValuePairs(item));
                    }
                    obj.PastQualification = pastCourseList;
                }


                if (result.Preferences != null && result.Preferences.Count > 0)
                {
                    List<List<Dictionary<string, object>>> preferenceList = new List<List<Dictionary<string, object>>>();
                    foreach (var item in result.Preferences)
                    {
                        preferenceList.Add(ConvertToKeyValuePairs(item));
                    }
                    obj.HotelPreferences = preferenceList;
                }
                else
                    obj.HotelPreferences = new List<string>();

                if (result.Documents != null)
                {
                    var keyValuePairs = new List<Dictionary<string, object>>();
                    foreach (var item in result.Documents)
                    {
                        var kvp = new Dictionary<string, object>
                            {
                               { "Key", item?.DocumentName },
                               { "Value", item?.FilePath }
                            };
                        keyValuePairs.Add(kvp);
                    }
                    obj.Documents = keyValuePairs;
                }
                else
                    obj.Documents = new List<string>();
            }
            return Ok(obj);
        }

        [HttpGet("{Id}/{Param}")]
        public async Task<IActionResult> Get(long Id, int Param = -1)
        {
            string[] arrRole = { "5", "6" };

            if (!arrRole.Contains(currentUserService.RoleEntityId)) {
                var IsValid = await applicationService.ApplicationValidate(Id);

                if (!IsValid)
                {
                    return BadRequest(new { Message = "No data found." });
                }

            }
            

            var result = await applicationService.GetByIdAsyncV2(Id, Param);
            switch (Param)
            {
                case 0:
                    if (result.AadharData != null)
                    {
                        if (!string.IsNullOrWhiteSpace(result.AadharData.UIDNo))
                        {
                            var qs = QueryStringEncryptDecrypt.DecryptQueryString(result.AadharData.UIDNo);
                            string UID = qs["UID"];
                            result.AadharData.UIDNo = !string.IsNullOrWhiteSpace(UID) ? "XXXXXXXX" + UID.Substring(8) : "";
                        }
                        var formattedAadharResult = result.AadharData.ToType<AadhaarPreviewResponseModel>();
                        return Ok(ConvertToKeyValuePairs(formattedAadharResult));
                    }
                    else
                        return Ok(new List<string>());
                case 1:
                    if (result.PersonalDetails != null)
                        return Ok(ConvertToKeyValuePairs(result.PersonalDetails));
                    else
                        return Ok(new List<string>());
                case 2:
                    if (result.CastDetails != null)
                        return Ok(ConvertToKeyValuePairs(result.CastDetails));
                    else
                        return Ok(new List<string>());
                case 3:
                    if (result.DomicileDetails != null)
                        return Ok(ConvertToKeyValuePairs(result.DomicileDetails));
                    else
                        return Ok(new List<string>());
                case 4:
                    if (result.PermanentAddress != null)
                        return Ok(ConvertToKeyValuePairs(result.PermanentAddress));
                    else
                        return Ok(new List<string>());
                case 5:
                    if (result.PresentAddress != null)
                        return Ok(ConvertToKeyValuePairs(result.PresentAddress));
                    else
                        return Ok(new List<string>());
                case 6:
                    if (result.ParentAddress != null)
                        return Ok(ConvertToKeyValuePairs(result.ParentAddress));
                    else
                        return Ok(new List<string>());
                case 7:
                    if (result.GuardianAddress != null)
                        return Ok(ConvertToKeyValuePairs(result.GuardianAddress));
                    else
                        return Ok(new List<string>());
                case 8:
                    if (result.OtherDetails != null)
                        return Ok(ConvertToKeyValuePairs(result.OtherDetails));
                    else
                        return Ok(new List<string>());
                case 9:
                    if (result.Preferences != null && result.Preferences.Count > 0)
                    {
                        List<List<Dictionary<string, object>>> preferenceList = new List<List<Dictionary<string, object>>>();
                        foreach (var item in result.Preferences)
                        {
                            preferenceList.Add(ConvertToKeyValuePairs(item));
                        }
                        return Ok(preferenceList);
                    }
                    else
                        return Ok(new List<string>());
                case 10:
                    List<List<Dictionary<string, object>>> currentCourseList = new List<List<Dictionary<string, object>>>();
                    if (result.CurrentCourse != null)
                    {
                        foreach (var item in result.CurrentCourse)
                        {
                            currentCourseList.Add(ConvertToKeyValuePairs(item));
                        }
                    }
                    else if (result.PreCurrentCourse != null && result.PreCurrentCourse.Count > 0)
                    {
                        foreach (var item in result.PreCurrentCourse)
                        {
                            currentCourseList.Add(ConvertToKeyValuePairs(item));
                        }
                    }
                    return Ok(currentCourseList);
                case 11:
                    List<List<Dictionary<string, object>>> pastCourseList = new List<List<Dictionary<string, object>>>();
                    if (result.PastCourse != null && result.PastCourse.Count > 0)
                    {
                        foreach (var item in result.PastCourse)
                        {
                            pastCourseList.Add(ConvertToKeyValuePairs(item));
                        }
                    }
                    else if (result.PrePastCourse != null && result.PrePastCourse.Count > 0)
                    {
                        foreach (var item in result.PrePastCourse)
                        {
                            pastCourseList.Add(ConvertToKeyValuePairs(item));
                        }
                    }
                    return Ok(pastCourseList);
                case 12:
                    var keyValuePairs = new List<Dictionary<string, object>>();
                    if (result.Documents != null)
                    {
                        foreach (var item in result.Documents)
                        {
                            var kvp = new Dictionary<string, object>
                            {
                               { "Key", item?.DocumentName },
                               { "Value", item?.FilePath }
                            };
                            keyValuePairs.Add(kvp);
                        }
                    }
                    return Ok(keyValuePairs);
                case -1:
                    var imageResult = new FIleViewModel();
                    if (result.AadharData != null)
                    {

                        if (!string.IsNullOrWhiteSpace(result.AadharData.AadhaarImage))
                        {
                            try
                            {
                                imageResult = await clientService.RequestSend<FIleViewModel>(HttpMethod.Get, $"{urloptions.UploadService}/File/{result.AadharData.AadhaarImage}", null);
                            }
                            catch (Exception)
                            {
                                imageResult = new FIleViewModel();
                            }
                        }
                    }
                    return Ok(new { AadhaarImage = imageResult, Headers = await applicationService.GetParams() });
                default:
                    return Ok(null);
            }
        }
        private List<Dictionary<string, object>> ConvertToKeyValuePairs(object obj)
        {
            var keyValuePairs = new List<Dictionary<string, object>>();
            var type = obj.GetType();
            foreach (PropertyInfo property in type.GetProperties())
            {
                string Key = string.Empty;
                var displayNameAttribute = property.GetCustomAttribute<DisplayNameAttribute>();
                if (displayNameAttribute != null)
                    Key = displayNameAttribute.DisplayName;
                else
                    Key = property.Name;
                object? value = property.GetValue(obj);
                var kvp = new Dictionary<string, object>
                {
                     { "Key", Key },
                     { "Value", value }
                };
                keyValuePairs.Add(kvp);
            }
            return keyValuePairs;
        }

        [HttpPost("Validate")]
        public async Task<IActionResult> Validate([FromBody] FilePathValidateModel model)
        {
            var data = await applicationService.ApplicationValidate(model.Id);
            if (data)
            {
                var result = await applicationService.ApplicationFileValidate(model.Id, model.FileId);
                try
                {
                    if (result == true)
                        return Ok(await clientService.RequestSendToGetFile<string>(HttpMethod.Get, $"{urloptions.UploadService}/File/{model.FileId}", null));
                    else
                        return Ok(new { HttpStatusCode = HttpStatusCode.OK, Data = new { Status = false, Message = "File not found." } });
                }
                catch (Exception ex)
                {
                    return Ok(new { HttpStatusCode = HttpStatusCode.OK, Data = new { Status = false, Message = "File not found." } });
                }
            }
            else
                return Ok(false);
        }
    }
}
