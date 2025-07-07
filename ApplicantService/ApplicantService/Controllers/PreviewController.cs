using ApplicantService.Helper;
using ApplicantService.Service;
using Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Model;
using Service.Implementation;
using Service.Interface;
using System.ComponentModel;
using System.Dynamic;
using System.Reflection;

namespace ApplicantService.Controllers
{
    public class PreviewController : APIBaseController
    {
        private readonly IProfileService profileService;
        private readonly IHttpClientService clientService;
        private readonly ILoginDetailsService loginDetailsService;
        private readonly APIUrl urloptions;
        public PreviewController(IProfileService profileService, IHttpClientService clientService, IOptions<APIUrl> urloptions, ILoginDetailsService loginDetailsService)
        {
            this.profileService = profileService;
            this.clientService = clientService;
            this.loginDetailsService = loginDetailsService;
            this.urloptions = urloptions.Value;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var validateresult = await profileService.ValidatePreviewData();
                if (!string.IsNullOrEmpty(validateresult) && validateresult.Split(":")[0] == "Success")
                {
                    var result = await profileService.Preview();

                    dynamic obj = new ExpandoObject();

                    if (result != null)
                    {
                        if (result.AadharData != null)
                        {
                            if (!string.IsNullOrWhiteSpace(result.AadharData.AadhaarImage))
                            {
                                var imageResult = new FIleViewModel();
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

                        if (result.CasteDetails != null)

                            obj.CasteDetails = ConvertToKeyValuePairs(result.CasteDetails);
                        else
                            obj.CasteDetails = new List<string>();

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

                        //if (result.PreCurrentCourse != null && result.PreCurrentCourse.Count > 0)
                        //{
                        //    List<List<Dictionary<string, object>>> currentCourseList = new List<List<Dictionary<string, object>>>();
                        //    foreach (var item in result.PreCurrentCourse)
                        //    {
                        //        currentCourseList.Add(ConvertToKeyValuePairs(item));
                        //    }
                        //    obj.CurrentCourse = currentCourseList;
                        //}

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
                        //else if (result.PrePastCourse != null && result.PrePastCourse.Count > 0)
                        //{
                        //    List<List<Dictionary<string, object>>> pastCourseList = new List<List<Dictionary<string, object>>>();
                        //    foreach (var item in result.PrePastCourse)
                        //    {
                        //        pastCourseList.Add(ConvertToKeyValuePairs(item));
                        //    }
                        //    obj.PastQualification = pastCourseList;
                        //}


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
                else
                {
                    return Ok(new { Status = false, Message = validateresult.Split(":")[1] });
                }
            }
            catch (Exception ex)
            {
                // ExceptionLogging.LogException(Convert.ToString(ex));
                return Ok(new { Status = false, ex.Message });
            }
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> Get(long Id)
        {
            try
            {  
                var result = await profileService.ApplicationDetails(Id);

                dynamic obj = new ExpandoObject();

                if (result != null)
                {
                    if (result.AadharData != null)
                    {
                        if (!string.IsNullOrWhiteSpace(result.AadharData.AadhaarImage))
                        {
                            var imageResult = new FIleViewModel();
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

                    if (result.CasteDetails != null)

                        obj.CasteDetails = ConvertToKeyValuePairs(result.CasteDetails);
                    else
                        obj.CasteDetails = new List<string>();

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
            catch (Exception ex)
            {
                // ExceptionLogging.LogException(Convert.ToString(ex));
                return Ok(new { Status = false, ex.Message });
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
                object value = property.GetValue(obj);
                var kvp = new Dictionary<string, object>
                {
                    { "Key", Key },
                    { "Value", value }
                };
                keyValuePairs.Add(kvp);
            }

            return keyValuePairs;
        }
    }
}
