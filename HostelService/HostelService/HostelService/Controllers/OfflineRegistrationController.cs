using Helper;
using HostelService.Helper;
using HostelService.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Model;
using Repository.Interface;
using Service.Implementation;
using Service.Interface;
using System.Text.Json;

namespace HostelService.Controllers;

public class OfflineRegistrationController : APIBaseController
{
    private readonly IOfflineRegistrationService offlineRegistrationService;
    private readonly IHttpClientService httpClientService;
    private readonly APIUrl urloptions;

    public OfflineRegistrationController(IOfflineRegistrationService offlineRegistrationService,
                                         IHttpClientService httpClientService,
                                         IOptions<APIUrl> urloptions)
    {
        this.offlineRegistrationService = offlineRegistrationService;
        this.httpClientService = httpClientService;
        this.urloptions = urloptions.Value;
    }

    [HttpGet("{Id}")]
    public async Task<IActionResult> Get(int? Id)
    {
        return Ok(await offlineRegistrationService.GetListAsync(Id));
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] OfflineRegistrationModel model)
    {
        List<OfflineAadhharList> success = new List<OfflineAadhharList>();
        List<OfflineAadhharList> error = new List<OfflineAadhharList>();
        List<OfflineAadhharList> objmodel = new List<OfflineAadhharList>();
        objmodel = model.aadhaarData;

        try
        {
            if (objmodel.Count > 0)
            {
                foreach (var item in objmodel)
                {
                    if (offlineRegistrationService.IsRecordValidate(item))
                    {
                        item.Message = "0";
                        item.EncryptedAadhar = QueryStringEncryptDecrypt.EncryptQueryString("UID=" + item.UIDNo);


                        success.Add(item);
                    }
                    else
                    {
                        item.Message = "Invalid UID Number";
                        error.Add(item);
                    }
                }
                await offlineRegistrationService.Registration(JsonSerializer.Serialize(success));
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

    [HttpPost("ExistingAadharData")]
    public async Task<IActionResult> ExistingAadharData([FromBody] OfflineRegistrationModel model)
    {
        List<OfflineAadhharList> success = new List<OfflineAadhharList>();
        List<OfflineAadhharList> error = new List<OfflineAadhharList>();
        List<OfflineAadhharList> objmodel = new List<OfflineAadhharList>();
        List<GetOfflineAadhharList> existingmodel = new List<GetOfflineAadhharList>();

        if (model.aadhaarData.Count > 0)
        {
            foreach (var item in model.aadhaarData)
            {
                OfflineAadhharList objmodel1 = new OfflineAadhharList();
                //var EncrypteUIDNo = QueryStringEncryptDecrypt.EncryptAadhaarNo(item.UIDNo);
                objmodel1.UIDNo = item.UIDNo;
                objmodel1.CourseType = item.CourseType;
                objmodel1.ApplicantName = item.ApplicantName;
                objmodel1.AdmissionYear = item.AdmissionYear;
                objmodel1.EncryptedAadhar = QueryStringEncryptDecrypt.EncryptQueryString("UID=" + item.UIDNo);
                objmodel.Add(objmodel1);
            }
            try
            {
                //var existStudentCount = await offlineRegistrationService.DirectVacancyExistingValue();
                List<GetOfflineAadhharList> existingAadharDataCount = await offlineRegistrationService.GetListOfflineExisting(null);
                if (existingAadharDataCount.Count > 0)
                {
                    return Ok(new { Status = false, SuccessRecords = new List<bool>(), ErrorrRecords = new List<bool>(), Message = "Aadhar data is already uploaded for this Hostel" });
                }

                List<GetOfflineAadhharList> existingAadharVerify = await offlineRegistrationService.GetListOfflineExisting(0);
                var validateAadhar = false;
                var wrongTypeOfCourse = false;
                List<OfflineAadhharList> duplicateAadhar = new List<OfflineAadhharList>();
                List<OfflineAadhharList> aadharCourseTypeMismatch = new List<OfflineAadhharList>();
                foreach (var item in objmodel)
                {
                    var Existingdata = existingAadharVerify.Where(y => item.EncryptedAadhar == y.EncryptedAadhar).ToList();
                    if (Existingdata.Count != 0)
                    {
                        OfflineAadhharList objmodel1 = new OfflineAadhharList();
                        objmodel1.UIDNo = item.UIDNo;
                        objmodel1.CourseType = item.CourseType;
                        objmodel1.ApplicantName = item.ApplicantName;
                        duplicateAadhar.Add(objmodel1);
                        validateAadhar = true;
                    }
                    var courseType = item?.CourseType?.ToLower();
                    if (courseType == "" && courseType != "Professional" && courseType != "NonProfessional" && courseType != "HigherSecondary" && courseType != "Secondary")
                    {
                        OfflineAadhharList objmodel1 = new OfflineAadhharList();
                        objmodel1.UIDNo = item.UIDNo;
                        objmodel1.CourseType = item.CourseType;
                        objmodel1.ApplicantName = item.ApplicantName;
                        aadharCourseTypeMismatch.Add(objmodel1);
                        wrongTypeOfCourse = true;
                    }
                }
                if (validateAadhar)
                {
                    return Ok(new { Status = false, SuccessRecords = new List<bool>(), ErrorrRecords = duplicateAadhar, Message = "Aadhar already uploaded by another warden." });
                }
                else if (wrongTypeOfCourse)
                {
                    return Ok(new { Status = false, SuccessRecords = new List<bool>(), wrongTypeOfCourse = duplicateAadhar, Message = "Type Of Course is worng." });
                }
                //else if (existStudentCount == null)
                //{
                //    return Ok(new { Status = false, SuccessRecords = new List<bool>(), ErrorrRecords = new List<bool>(), Message = "Direct vacancy not uploaded or assistance commissioner not approved, so can't upload existing Aadhar data." });
                //}
                //else if ((existStudentCount.exitingStudentCount + existStudentCount.newStudentCount) != objmodel.Count)
                //{
                //    return Ok(new { Status = false, SuccessRecords = new List<bool>(), ErrorrRecords = new List<bool>(), Message = "Uploaded Aadhar and existing student data in direct vacancy doesn't match." });
                //}
                else if (model.FilePath == null || model.FilePath == "")
                {
                    return Ok(new { Status = false, SuccessRecords = new List<bool>(), ErrorrRecords = new List<bool>(), Message = "Uploaded Aadhar confirmation PDF." });
                }
                else
                {
                    //existingmodel = await offlineRegistrationService.GetListOffline(0);
                    //var data = existingmodel.Where(y => objmodel.Any(z => z.EncryptedAadhar == y.EncryptedAadhar)).ToList();
                    //if (data.Count == 0)
                    //{

                    if (objmodel.Count > 0)
                    {
                        foreach (var item in objmodel)
                        {
                            if (offlineRegistrationService.IsRecordValidate(item))
                            {
                                item.Message = "";
                                item.UIDNo = QueryStringEncryptDecrypt.EncryptAadhaarNo(item.UIDNo);
                                success.Add(item);
                            }
                            else
                            {
                                item.Message = "Invalid UID Number";
                                error.Add(item);
                            }
                        }
                        await offlineRegistrationService.ExistingAadharRegistration(JsonSerializer.Serialize(success), model.FilePath);
                        return Ok(new { Status = true, SuccessRecords = success, ErrorrRecords = error, Message = "Records updated." });
                    }
                    else
                        return Ok(new { Status = false, SuccessRecords = new List<bool>(), ErrorrRecords = new List<bool>(), Message = "No records found to update." });
                    //}
                    //else
                    //{
                    //    //List<string?> duplicateAadhar = new List<string?>();
                    //    //foreach (var item in data)
                    //    //{
                    //    //    OfflineAadhharList objmodel1 = new OfflineAadhharList();
                    //    //    objmodel1.UIDNo = item.UIDNo;
                    //    //    duplicateAadhar.Add(objmodel1);
                    //    //    //duplicateAadhar.Add(item.UIDNo);
                    //    //}
                    //    return Ok(new { Status = false, SuccessRecords = new List<bool>(), ErrorrRecords = data, Message = "Aadhar alreredy uploaded as new applicant, Please Remove that aadhar and upload again." });
                    //}
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(Convert.ToString(ex));
                return Ok(new { Status = false, SuccessRecords = new List<bool>(), ErrorrRecords = new List<bool>(), Message = ex.Message });
            }
        }
        else
            return Ok(new { Status = false, SuccessRecords = new List<bool>(), ErrorrRecords = new List<bool>(), Message = "No records found to update." });
    }

    [HttpPost("ExistingAadharDataList")]
    public async Task<IActionResult> Post([FromBody] SearchExistingAadharDataModel model)
    {
        return Ok(await offlineRegistrationService.GetListAsync(model));
    }

    [HttpPost("EncrypAadharData")]
    public async Task<IActionResult> EncrypAadharData()
    {


        try
        {
            var allUIDNo = await offlineRegistrationService.AdminQuery("select UIDNo from offline_allowed_applicants where EncryptedAadhar is null limit 1000; ", "adminquery");

            foreach (var item in allUIDNo)
            {
                string adharNo = QueryStringEncryptDecrypt.EncryptQueryString("UID=" + item.UIDNo);
                await offlineRegistrationService.UpdateAdminQuery(" Update offline_allowed_applicants set EncryptedAadhar = '" + adharNo + "' where UIDNo = '" + item.UIDNo + "' ", "adminupdate");

            }
            return Ok(new { Status = true, Message = "Records  updated successfully " });

        }
        catch (Exception ex)
        {
            ExceptionLogging.LogException(Convert.ToString(ex));
            return Ok(new { Status = false, SuccessRecords = new List<bool>(), ErrorrRecords = new List<bool>(), Message = ex.Message });
        }
    }

    [HttpPost("DeleteExistingAadharData")]
    public async Task<IActionResult> DeleteExistingAadharData([FromBody] SearchExistingAadharDataModel model)
    {
        try
        {
            var result = await offlineRegistrationService.DeleteExistingAadharRegistration(model.HostelId);
            if (result == "Success")
            {
                return Ok(new { Status = true, Message = "Record Deleted Successfuly." });
            }
            else
            {
                return Ok(new { Status = false, Message = "No records found to delete." });
            }
        }
        catch (Exception ex)
        {
            ExceptionLogging.LogException(Convert.ToString(ex));
            return Ok(new { Status = false, SuccessRecords = new List<bool>(), ErrorrRecords = new List<bool>(), Message = "No records found to update." });
        }
    }

    [HttpPost("DeletedExistingAadharDataList")]
    public async Task<IActionResult> Post([FromBody] SearchDeleteExistingAadharDataModel model)
    {
        return Ok(await offlineRegistrationService.GetListAsync(model));
    }

    [HttpGet("DeletedExistingAadharId")]
    public async Task<IActionResult> PostDeletedExistingAadharId(int? Id, string? HostelId)
    {
        var result = await offlineRegistrationService.DeleteExistingAadhaarId(Id, HostelId);
        if (result == "Success")
        {
            return Ok(new { Status = true, Message = "Record Deleted Successfuly." });
        }
        else
        {
            return Ok(new { Status = false, Message = "No records found to delete." });
        }
    }
}