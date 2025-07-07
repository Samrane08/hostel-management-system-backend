using Dapper;
using Helper;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Model;
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.X9;
using Repository.Interface;
using Service.Interface;
using System.Data;
using System.Linq;

namespace Service.Implementation;

public class ProfileService : IProfileService
{
    private readonly IDapper dapper;
    private readonly ICurrentUserService currentUserService;
    private readonly IErrorLogger errorLogger;
    public ProfileService(IDapper dapper, ICurrentUserService currentUserService, IErrorLogger errorLogger)
    {
        this.dapper = dapper;
        this.currentUserService = currentUserService;
        this.errorLogger = errorLogger;
    }
    //test
    public async Task<PersonalDetailsModel> GetPersonalDetails()
    {
        try
        {
            var param = new DynamicParameters();
            param.Add("p_UserId", currentUserService.UserNumericId, DbType.Int64);
            var result = await Task.FromResult(dapper.Get<PersonalDetailsModel>("usp_GetApplicantPersonalDetails", param, commandType: CommandType.StoredProcedure));
            return result;
        }
        catch (Exception ex)
        {
            ExceptionLogging.LogException(Convert.ToString(ex));
            await errorLogger.Log("usp_GetApplicantPersonalDetails", ex);
            return new PersonalDetailsModel();
        }
    }

    public async Task<PersonalDetailsModel> GetAttendanceDetailsList()
    {
        try
        {
            var param = new DynamicParameters();
            param.Add("p_UserId", currentUserService.UserNumericId, DbType.Int64);
            var result = await Task.FromResult(dapper.Get<PersonalDetailsModel>("usp_GetApplicantAttendaceDetails", param, commandType: CommandType.StoredProcedure));
            return result;
        }
        catch (Exception ex)
        {
            ExceptionLogging.LogException(Convert.ToString(ex));
            await errorLogger.Log("usp_GetApplicantAttendaceDetails", ex);
            return new PersonalDetailsModel();
        }
    }
    
    public async Task<PersonalDetailsModel> SavePersonalDetails(PersonalDetailsModel model)
    {
        try
        {
            var param = new DynamicParameters();
            param.Add("p_ServiceType", model.ServiceType, DbType.Int32);
            param.Add("p_IsNewApplicant", model.IsNewApplicant, DbType.Boolean);
            param.Add("p_TypeOfCourse", model.TypeOfCourse, DbType.Int32);
            param.Add("p_HostelId", model.HostelId, DbType.Int32);
            param.Add("p_ApplicantName", model.ApplicantName, DbType.String);
            param.Add("p_DOB", model.DOB, DbType.String);
            param.Add("p_Age", model.Age, DbType.Int32);
            param.Add("p_Gender", model.Gender, DbType.Int32);
            param.Add("p_MaritalStatus", model.MaritalStatus, DbType.Int32);
            param.Add("p_IsOrphan", model.IsOrphan, DbType.Boolean);
            param.Add("p_FatherName", model.FatherName, DbType.String);
            param.Add("p_MotherName", model.MotherName, DbType.String);
            param.Add("p_GuardianName", model.GuardianName, DbType.String);
            param.Add("p_IsFatherAlive", model.IsFatherAlive, DbType.Boolean);
            param.Add("p_IsMotherAlive", model.IsMotherAlive, DbType.Boolean);
            param.Add("p_CasteCategory", model.CasteCategory, DbType.Int32);
            param.Add("p_Caste", model.Caste, DbType.Int32);
            param.Add("p_CertificateNumber", model.CertificateNumber, DbType.String);
            param.Add("p_CertificateIssueDistrict", model.CertificateIssueDistrict, DbType.Int32);
            param.Add("p_IssuingAuthority", model.IssuingAuthority, DbType.Int32);
            param.Add("p_IssueDate", model.IssueDate, DbType.DateTime);
            param.Add("p_IsMaharastraDomicile", model.IsMaharastraDomicile, DbType.Boolean);
            param.Add("p_DomicileCertificateNo", model.DomicileCertificateNo, DbType.String);
            param.Add("p_DomicileIssuingAuthority", model.DomicileIssuingAuthority, DbType.Int32);
            param.Add("p_CastCertificateApplicantName", model.CastCertificateApplicantName, DbType.String);
            param.Add("p_UserId", currentUserService.UserNumericId, DbType.Int64);
            param.Add("p_CreatedBy", currentUserService.UserId, DbType.String);
            var result = await Task.FromResult(dapper.Insert<PersonalDetailsModel>("usp_UpsertApplicantPersonalDetails", param, commandType: CommandType.StoredProcedure));
            return result;
        }
        catch (Exception ex)
        {
            ExceptionLogging.LogException(Convert.ToString(ex));
            await errorLogger.Log("usp_UpsertApplicantPersonalDetails", ex);
            return new PersonalDetailsModel();
        }
    }
    public async Task<List<ViewDocumentModel>> GetUploadDocuments()
    {
        try
        {
            var param = new DynamicParameters();
            param.Add("p_UserId", currentUserService.UserNumericId, DbType.Int64);
            var result = await Task.FromResult(dapper.GetAll<ViewDocumentModel>("usp_GetApplicantDocuments", param, commandType: CommandType.StoredProcedure));

            if (result.Count > 0)
            {
                var unrequirdeId = new List<int>();
                var personaldetails = await GetPersonalDetails();
                if (personaldetails != null && personaldetails.CasteCategory != null)
                {
                    var categoryIds = new List<int>(); categoryIds.Add(1); categoryIds.Add(6); categoryIds.Add(8);

                    if (categoryIds.Contains((int)personaldetails.CasteCategory))
                    {
                        unrequirdeId.Add(1);
                    }
                    if (personaldetails.IsMaharastraDomicile != true)
                    {
                        unrequirdeId.Add(2);
                    }

                    if (personaldetails.ServiceType != 2)
                    {

                        var a = result.Where(x => x.Id == 11).FirstOrDefault();
                        if (a != null)
                        {
                            result.Remove(a);
                        }
                        var b = result.Where(x => x.Id == 12).FirstOrDefault();
                        if (b != null)
                        {
                            result.Remove(b);
                        }
                        var c = result.Where(x => x.Id == 13).FirstOrDefault();
                        if (c != null)
                        {
                            result.Remove(c);
                        }
                    }
                }

                var otherDetails = await GetOtherDetails();
                if (otherDetails != null)
                {
                    if (otherDetails.IsApplicantDisable != 1)
                    {
                        unrequirdeId.Add(4);
                    }
                }
                foreach (var item in result)
                {
                    if (unrequirdeId.Contains(item.Id))
                        item.IsRequired = false;
                }
            }

            return result;
        }
        catch (Exception ex)
        {
            ExceptionLogging.LogException(Convert.ToString(ex));
            return new List<ViewDocumentModel>();
        }
    }
    public async Task<bool> DoumentUpload(DocumentUploadModel model)
    {
        try
        {
            var param = new DynamicParameters();
            param.Add("p_DocumentId", model.DocumentId, DbType.Int32);
            param.Add("p_FileKey", model.FileKey, DbType.String);
            param.Add("p_UserId", currentUserService.UserNumericId, DbType.Int64);
            param.Add("p_CreatedBy", currentUserService.UserId, DbType.String);
            var result = await Task.FromResult(dapper.Execute("usp_UploadApplicantDocuments", param, commandType: CommandType.StoredProcedure));
            return true;
        }
        catch (Exception ex)
        {
            ExceptionLogging.LogException(Convert.ToString(ex));
            await errorLogger.Log("usp_UploadApplicantDocuments", ex);
            return false;
        }
    }
    public async Task<AddressDetailsModel> GetAddressDetails()
    {
        try
        {
            var myParams = new DynamicParameters();
            myParams.Add("p_UserId", currentUserService.UserNumericId, DbType.Int64);
            var result = await Task.FromResult(dapper.Insert<AddressDetailsModel>("usp_GetApplicantAddressDetails", myParams, commandType: CommandType.StoredProcedure));
            return result;
        }
        catch (Exception ex)
        {
            ExceptionLogging.LogException(Convert.ToString(ex));
            await errorLogger.Log("usp_GetApplicantAddressDetails", ex);
            return new AddressDetailsModel();
        }
    }
    public async Task<AddressDetailsModel> SaveAddressDetails(AddressDetailsModel model)
    {
        try
        {
            var param = new DynamicParameters();
            param.Add("p_PermanentAddress1", model.PermanentAddress1, DbType.String);
            param.Add("p_PermanentAddress2", model.PermanentAddress2, DbType.String);
            param.Add("p_PermanentAddress3", model.PermanentAddress3, DbType.String);
            param.Add("p_PermanentState", model.PermanentState, DbType.Int16);
            param.Add("p_PermanentDistrict", model.PermanentDistrict, DbType.Int16);
            param.Add("p_PermanentTaluka", model.PermanentTaluka, DbType.Int16);
            param.Add("p_PermanentVillage", model.PermanentVillage, DbType.Int32);
            param.Add("p_PermanentPincode", model.PermanentPincode, DbType.String);
            param.Add("p_IsPresentSameAsPermanent", model.IsPresentSameAsPermanent, DbType.Boolean);
            param.Add("p_PresentAddress1", model.PresentAddress1, DbType.String);
            param.Add("p_PresentAddress2", model.PresentAddress2, DbType.String);
            param.Add("p_PresentAddress3", model.PresentAddress3, DbType.String);
            param.Add("p_PresentState", model.PresentState, DbType.Int16);
            param.Add("p_PresentDistrict", model.PresentDistrict, DbType.Int16);
            param.Add("p_PresentTaluka", model.PresentTaluka, DbType.Int16);
            param.Add("p_PresentVillage", model.PresentVillage, DbType.Int32);
            param.Add("p_PresentPincode", model.PresentPincode, DbType.String);
            param.Add("p_UserId", currentUserService.UserNumericId, DbType.Int64);
            param.Add("p_CreatedBy", currentUserService.UserId, DbType.String);
            var result = await Task.FromResult(dapper.Insert<AddressDetailsModel>("usp_UpsertApplicantAddressDetails", param, commandType: CommandType.StoredProcedure));
            return result;
        }
        catch (Exception ex)
        {
            ExceptionLogging.LogException(Convert.ToString(ex));
            await errorLogger.Log("usp_UpsertApplicantAddressDetails", ex);
            return new AddressDetailsModel();
        }
    }
    public async Task<ParentAddressModel> GetParentAddressDetails()
    {
        try
        {
            var myParams = new DynamicParameters();
            myParams.Add("p_UserId", currentUserService.UserNumericId, DbType.Int64);
            var result = await Task.FromResult(dapper.Get<ParentAddressModel>("usp_GetApplicantAddressDetails", myParams, commandType: CommandType.StoredProcedure));
            return result;
        }
        catch (Exception ex)
        {
            ExceptionLogging.LogException(Convert.ToString(ex));
            await errorLogger.Log("usp_GetApplicantAddressDetails", ex);
            return new ParentAddressModel();
        }
    }
    public async Task<ParentAddressModel> SaveParentAddressDetails(ParentAddressModel model)
    {
        try
        {
            var param = new DynamicParameters();
            param.Add("p_ParentsPresentAddress1", model.ParentsPresentAddress1, DbType.String);
            param.Add("p_ParentsPresentAddress2", model.ParentsPresentAddress2, DbType.String);
            param.Add("p_ParentsPresentAddress3", model.ParentsPresentAddress3, DbType.String);
            param.Add("p_ParentsPresentState", model.ParentsPresentState, DbType.Int16);
            param.Add("p_ParentsPresentDistrict", model.ParentsPresentDistrict, DbType.Int16);
            param.Add("p_ParentsPresentTaluka", model.ParentsPresentTaluka, DbType.Int16);
            param.Add("p_ParentsPresentVillage", model.ParentsPresentVillage, DbType.Int32);
            param.Add("p_ParentsPresentPincode", model.ParentsPresentPincode, DbType.String);
            param.Add("p_ParentsEmailId", model.ParentsEmailId, DbType.String);
            param.Add("p_ParentsMobileNo", model.ParentsMobileNo, DbType.String);
            param.Add("p_IsGuardianSameAsParent", model.IsGuardianSameAsParent, DbType.Boolean);
            param.Add("p_GuardianAddress1", model.GuardianAddress1, DbType.String);
            param.Add("p_GuardianAddress2", model.GuardianAddress2, DbType.String);
            param.Add("p_GuardianAddress3", model.GuardianAddress3, DbType.String);
            param.Add("p_GuardianState", model.GuardianState, DbType.Int16);
            param.Add("p_GuardianDistrict", model.GuardianDistrict, DbType.Int16);
            param.Add("p_GuardianTaluka", model.GuardianTaluka, DbType.Int16);
            param.Add("p_GuardianVillage", model.GuardianVillage, DbType.Int32);
            param.Add("p_GuardianPincode", model.GuardianPincode, DbType.String);
            param.Add("p_GuardianEmailId", model.GuardianEmailId, DbType.String);
            param.Add("p_GuardianMobileNo", model.GuardianMobileNo, DbType.String);
            param.Add("p_UserId", currentUserService.UserNumericId, DbType.Int64);
            param.Add("p_CreatedBy", currentUserService.UserId, DbType.String);
            var result = await Task.FromResult(dapper.Insert<ParentAddressModel>("usp_UpsertApplicantParentAddressDetails", param, commandType: CommandType.StoredProcedure));
            return result;
        }
        catch (Exception ex)
        {
            ExceptionLogging.LogException(Convert.ToString(ex));
            await errorLogger.Log("usp_UpsertApplicantParentAddressDetails", ex);
            return new ParentAddressModel();
        }
    }
    public async Task SaveHostelPreference(List<HostelPreferenceModel> models)
    {

        try
        {
            //check whether Hostel pref is set or not 
            var existing = await GetHostelPreference(1);
            if (existing.Count > 0 && 0 != existing.Select(x => x.HostelId).FirstOrDefault())
            {
                foreach (var model in models)
                {
                    var s = existing.Where(x => x.HostelId == model.HostelId && x.Preference == model.Preference).FirstOrDefault();
                    if (s != null)
                    {
                        existing.Remove(s);
                    }
                }
            }

           
            if (existing.Count > 0 && 0 != existing.Select(x => x.HostelId).FirstOrDefault())

                 //check whether Hostel pref is set or not
                foreach (var item in existing)
                {
                    try
                    {
                        var myParams = new DynamicParameters();
                        myParams.Add("p_Id", item.Id, DbType.Int64);
                        await Task.FromResult(dapper.Execute("usp_ApplicantPreferenceInactive", myParams, commandType: CommandType.StoredProcedure));
                    }
                    catch (Exception ex)
                    {
                        await errorLogger.Log("usp_ApplicantPreferenceInactive", ex);
                    }
                }

            foreach (var item in models)
            {
                try
                {
                    var myParams = new DynamicParameters();
                    myParams.Add("p_UserId", currentUserService.UserNumericId, DbType.Int64);
                    myParams.Add("p_HostelId", item.HostelId, DbType.Int32);
                    myParams.Add("p_Preference", item.Preference, DbType.Int32);
                    myParams.Add("p_CreatedBy", currentUserService.UserId, DbType.String);
                    await Task.FromResult(dapper.Execute("usp_ApplicantPreferenceSet", myParams, commandType: CommandType.StoredProcedure));
                }
                catch (Exception ex)
                {
                    await errorLogger.Log("usp_ApplicantPreferenceSet", ex);
                }

            }
        }
        catch (Exception ex)
        {
            ExceptionLogging.LogException(Convert.ToString(ex));
            await errorLogger.Log("SaveHostelPreference Method", ex);
        }
    }
    public async Task SaveHostelPreferenceV2(List<HostelPreferenceModel> models)
    {
        try
        {
            var jsonArray = JsonConvert.SerializeObject(models.Select(x => new { hostelId = x.HostelId, preference = x.Preference }).ToList());
            var myParams = new DynamicParameters();
            myParams.Add("p_UserId", currentUserService.UserNumericId, DbType.Int64);
            myParams.Add("@jsonArray", jsonArray, DbType.String);
            myParams.Add("p_CreatedBy", currentUserService.UserId, DbType.String);
            await Task.FromResult(dapper.Execute("usp_ApplicantPreferenceSet_V2", myParams, commandType: CommandType.StoredProcedure));
        }
        catch (Exception ex)
        {
            ExceptionLogging.LogException(Convert.ToString(ex));
            await errorLogger.Log("usp_ApplicantPreferenceSet_V2", ex);
        }
    }
    public async Task<List<HostelPreferenceModel>> GetHostelPreference(int? Lang)
    {
        try
        {
            var param = new DynamicParameters();
            param.Add("p_UserId", currentUserService.UserNumericId, DbType.Int64);
            param.Add("p_Lang", Lang, DbType.Int32);
            var result = await Task.FromResult(dapper.GetAll<HostelPreferenceModel>("usp_GetHostelPreferences", param, commandType: CommandType.StoredProcedure));
            return result;
        }
        catch (Exception ex)
        {
            ExceptionLogging.LogException(Convert.ToString(ex));
            await errorLogger.Log("usp_GetHostelPreferences", ex);
            return new List<HostelPreferenceModel>();
        }
    }
    public async Task<OtherDetailsModel> GetOtherDetails()
    {
        try
        {
            var myParams = new DynamicParameters();
            myParams.Add("p_UserId", currentUserService.UserNumericId, DbType.Int64);
            var result = await Task.FromResult(dapper.Get<OtherDetailsModel>("usp_GetApplicantOtherDetails", myParams, commandType: CommandType.StoredProcedure));
            return result;
        }
        catch (Exception ex)
        {
            ExceptionLogging.LogException(Convert.ToString(ex));
            await errorLogger.Log("usp_GetApplicantOtherDetails", ex);
            return new OtherDetailsModel();
        }
    }
    public async Task<OtherDetailsModel> SaveOtherDetails(OtherDetailsModel model)
    {
        try
        {
            var param = new DynamicParameters();
            param.Add("p_isAvailedMaintainaceAllowance", model.isAvailedMaintainaceAllowance, DbType.Int16);
            param.Add("p_AvailedMaintainaceAllowanceUserId", model.AvailedMaintainaceAllowanceUserId, DbType.String);
            param.Add("p_IsApplicantDisable", model.IsApplicantDisable, DbType.Int16);
            param.Add("p_DisabilityPercentage", model.DisabilityPercentage, DbType.String);
            param.Add("p_IsCourseAvailableInDistrict", model.IsCourseAvailableInDistrict, DbType.Int16);
            param.Add("p_IsLivedInGovermentHostel", model.IsLivedInGovermentHostel, DbType.Int16);
            param.Add("p_NameOfHostel", model.NameOfHostel, DbType.Int16);
            param.Add("p_HostelDistrict", model.HostelDistrict, DbType.Int16);
            param.Add("p_DurationFrom", model.DurationFrom, DbType.DateTime);
            param.Add("p_DurationTo", model.DurationTo, DbType.DateTime);
            param.Add("p_IsMaterialsReturn", model.IsMaterialsReturn, DbType.Int16);
            param.Add("p_UserId", currentUserService.UserNumericId, DbType.Int64);
            param.Add("p_CreatedBy", currentUserService.UserId, DbType.String);
            var result = await Task.FromResult(dapper.Insert<OtherDetailsModel>("usp_UpsertApplicantOtherDetails", param, commandType: CommandType.StoredProcedure));
            return result;
        }
        catch (Exception ex)
        {
            ExceptionLogging.LogException(Convert.ToString(ex));
            return new OtherDetailsModel();
        }
    }
    public async Task<List<SelectListModel>> GetHostelList(int? Lang)
    {
        try
        {
            var myParams = new DynamicParameters();
            myParams.Add("p_UserId", currentUserService.UserNumericId, DbType.Int64);
            myParams.Add("p_Lang", Lang, DbType.Int32);
            var result = await Task.FromResult(dapper.GetAll<SelectListModel>("usp_GetHostelListByApplicantCourseDistrict", myParams, commandType: CommandType.StoredProcedure));
            return result;
        }
        catch (Exception ex)
        {
            ExceptionLogging.LogException(Convert.ToString(ex));
            await errorLogger.Log("usp_GetHostelListByApplicantCourseDistrict", ex);
            return new List<SelectListModel>();
        }
    }
    public async Task<PreviewModel> Preview()
    {
        var response = new PreviewModel();
        try
        {
            var myParams = new DynamicParameters();
            myParams.Add("p_UserId", currentUserService.UserNumericId, DbType.Int64);
            var result = await dapper.MultiResult("usp_ApplicantProfilePreview", myParams, commandType: CommandType.StoredProcedure);
            if (result.Count > 0)
            {
                if (result[0] != null)
                {
                    var data = JsonConvert.SerializeObject(result[0].FirstOrDefault());
                    response.AadharData = JsonConvert.DeserializeObject<AadhaarDetailsPreviewModel>(data);
                }
                if (result[1] != null)
                {
                    var data = JsonConvert.SerializeObject(result[1].FirstOrDefault());
                    response.PersonalDetails = JsonConvert.DeserializeObject<PersonalDetailsPreviewModel>(data);
                }
                if (result[2] != null)
                {
                    var data = JsonConvert.SerializeObject(result[2].FirstOrDefault());
                    response.CastDetails = JsonConvert.DeserializeObject<CastDetailsPreviewModel>(data);

                    var data2 = JsonConvert.SerializeObject(result[2].FirstOrDefault());
                    response.DomicileDetails = JsonConvert.DeserializeObject<DomicilePreviewModel>(data2);
                }
                if (result[3] != null)
                {
                    var data = JsonConvert.SerializeObject(result[3].FirstOrDefault());
                    response.PermanentAddress = JsonConvert.DeserializeObject<AddressDetailsPreviewModel>(data);
                }
                if (result[4] != null)
                {
                    var data = JsonConvert.SerializeObject(result[4].FirstOrDefault());
                    response.PresentAddress = JsonConvert.DeserializeObject<AddressDetailsPreviewModel>(data);
                }
                if (result[5] != null)
                {
                    var data = JsonConvert.SerializeObject(result[5].FirstOrDefault());
                    response.ParentAddress = JsonConvert.DeserializeObject<AddressDetails2PreviewModel>(data);
                }
                if (result[6] != null)
                {
                    var data = JsonConvert.SerializeObject(result[6].FirstOrDefault());
                    response.GuardianAddress = JsonConvert.DeserializeObject<AddressDetails2PreviewModel>(data);
                }
                if (result[7] != null)
                {
                    var data = JsonConvert.SerializeObject(result[7].FirstOrDefault());
                    response.OtherDetails = JsonConvert.DeserializeObject<OtherDetailsPreviewModel>(data);
                }
                if (result[8] != null)
                {
                    var data = JsonConvert.SerializeObject(result[8].ToList());
                    response.Preferences = JsonConvert.DeserializeObject<List<HostelPreferencePreviewModel>>(data);
                }
                if (result[9] != null)
                {
                    var data = JsonConvert.SerializeObject(result[9].ToList());
                    response.Documents = JsonConvert.DeserializeObject<List<DocumentPreviewModel>>(data);
                }
                if (result[10] != null)
                {
                    var data = JsonConvert.SerializeObject(result[10].ToList());
                    response.CurrentCourse = JsonConvert.DeserializeObject<List<CurrentCoursePreviewModel>>(data);
                }
                if (result[11] != null)
                {
                    var data = JsonConvert.SerializeObject(result[11].ToList());
                    response.PastCourse = JsonConvert.DeserializeObject<List<PastCoursePreviewModel>>(data);
                }
                if (result[12] != null)
                {
                    var data = JsonConvert.SerializeObject(result[12].ToList());
                    response.PreCurrentCourse = JsonConvert.DeserializeObject<List<PreCurrentCoursePreviewModel>>(data);
                }
                if (result[13] != null)
                {
                    var data = JsonConvert.SerializeObject(result[13].ToList());
                    response.PrePastCourse = JsonConvert.DeserializeObject<List<PrePastCoursePreviewModel>>(data);
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionLogging.LogException(Convert.ToString(ex));
            await errorLogger.Log("usp_ApplicantProfilePreview", ex);
        }
        return response;
    }
    public async Task<string?> ApplicationIdGenerator()
    {
        try
        {
            var param = new DynamicParameters();
            param.Add("p_UserId", currentUserService.UserNumericId, DbType.Int64);
            var result = await Task.FromResult(dapper.Get<ApplicationNumberGeneratorModel>("usp_GenerateApplicationNumber", param, commandType: CommandType.StoredProcedure));
            return result.ApplicationId;
        }
        catch (Exception ex)
        {
            ExceptionLogging.LogException(Convert.ToString(ex));
            await errorLogger.Log("usp_GenerateApplicationNumber", ex);
            return null;
        }
    }
    public async Task<ApplicationModel?> HostelApply(string? ApplicationId)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(ApplicationId))
                ApplicationId = await ApplicationIdGenerator();
            var param = new DynamicParameters();
            param.Add("p_ApplicationNo", ApplicationId, DbType.String);
            param.Add("p_UserId", currentUserService.UserNumericId, DbType.Int64);
            param.Add("p_CreatedBy", currentUserService.UserId, DbType.String);
            var result = await Task.FromResult(dapper.Insert<ApplicationModel>("usp_ApplyForHostel", param, commandType: CommandType.StoredProcedure));
            if (result != null) result.UserId = currentUserService.UserNumericId;
            return result;
        }
        catch (Exception ex)
        {
            ExceptionLogging.LogException(Convert.ToString(ex));
            await errorLogger.Log("usp_ApplyForHostel", ex);
            return null;
        }
    }
    public async Task<ProfileCompleteValidationModel> ValidateProfile()
    {
        try
        {
            var param = new DynamicParameters();
            param.Add("p_UserId", currentUserService.UserNumericId, DbType.Int64);
            var result = await Task.FromResult(dapper.Get<ProfileCompleteValidationModel>("usp_ValidateProfile", param, commandType: CommandType.StoredProcedure));
            return result;
        }
        catch (Exception ex)
        {
            ExceptionLogging.LogException(Convert.ToString(ex));
            await errorLogger.Log("usp_ValidateProfile", ex);
            return null;
        }
    }
    public async Task<ProfileCompleteValidationModel> ProfileStatus()
    {
        try
        {
            var param = new DynamicParameters();
            param.Add("p_UserId", currentUserService.UserNumericId, DbType.Int64);
            param.Add("p_CreatedBy", currentUserService.UserId, DbType.String);
            var result = await Task.FromResult(dapper.Get<ProfileCompleteValidationModel>("usp_CheckProfileCompleteStatus", param, commandType: CommandType.StoredProcedure));
            return result;
        }
        catch (Exception ex)
        {
            ExceptionLogging.LogException(Convert.ToString(ex));
            await errorLogger.Log("usp_CheckProfileCompleteStatus", ex);
            return null;
        }
    }
    public async Task<ApplicationModel?> GetPaymentParams()
    {
        try
        {
            var param = new DynamicParameters();
            param.Add("p_UserId", currentUserService.UserNumericId, DbType.Int64);
            var result = await Task.FromResult(dapper.Get<ApplicationModel>("usp_GetPaymentParams", param, commandType: CommandType.StoredProcedure));
            if (result != null) result.UserId = currentUserService.UserNumericId;
            return result;
        }
        catch (Exception ex)
        {
            ExceptionLogging.LogException(Convert.ToString(ex));
            await errorLogger.Log("usp_GetPaymentParams", ex);
            return null;
        }
    }
    public async Task<List<ApplicationMainModel>?> GetApplicationList()
    {
        try
        {
            var appList = new List<ApplicationMainModel>();
            var hostelList = new List<ApplicationModel2>();
            var myParams = new DynamicParameters();
            myParams.Add("p_UserId", currentUserService.UserNumericId, DbType.Int64);
            var result = await dapper.MultiResult("usp_ApplicationStatus", myParams, commandType: CommandType.StoredProcedure);

            if (result.Count > 0)
            {
                if (result[0] != null)
                {
                    var data = JsonConvert.SerializeObject(result[0].ToList());
                    appList = JsonConvert.DeserializeObject<List<ApplicationMainModel>>(data);
                }
                if (result[1] != null)
                {
                    var data = JsonConvert.SerializeObject(result[1].ToList());
                    hostelList = JsonConvert.DeserializeObject<List<ApplicationModel2>>(data);
                }
            }

            if (appList != null && appList.Count > 0)
            {
                var Paymentparam = await GetPaymentParams();
                foreach (var item in appList)
                {
                    item.PaymentParam = new ApplicationModel
                    {
                        AppId = item.ApplicationNo,
                        Email = Paymentparam?.Email,
                        Message = Paymentparam?.Message,
                        MobileNo = Paymentparam?.MobileNo,
                        Name = item.Name,
                        UserId = currentUserService.UserNumericId,
                    };
                    item.HostelList = hostelList?.Where(x => x.ApplicationId == item.Id).ToList();
                }
            }
            return appList;
        }
        catch (Exception ex)
        {
            ExceptionLogging.LogException(Convert.ToString(ex));
            await errorLogger.Log("usp_ApplicationStatus", ex);
            return null;
        }
    }
    public async Task<string?> CancelApplication(string ApplicationNo)
    {
        try
        {
            var param = new DynamicParameters();
            param.Add("p_ApplicationNo", ApplicationNo, DbType.String);
            param.Add("p_UserId", currentUserService.UserNumericId, DbType.Int64);
            var result = await Task.FromResult(dapper.Get<string>("usp_CancelApplication", param, commandType: CommandType.StoredProcedure));
            return result;
        }
        catch (Exception ex)
        {
            ExceptionLogging.LogException(Convert.ToString(ex));
            await errorLogger.Log("usp_CancelApplication", ex);
            return null;
        }
    }
    public async Task<int?> Acceptallocation(HostelAcceptanceModel model)
    {
        try
        {
            var param = new DynamicParameters();
            param.Add("p_ApplicationNo", model.ApplicationNo, DbType.String);
            param.Add("p_Hostelid", model.Hostelid, DbType.String);
            param.Add("p_PreferenceId", model.Preference, DbType.String);
            var result = await Task.FromResult(dapper.Get<int>("usp_Acceptallocation", param, commandType: CommandType.StoredProcedure));
            return result;
        }
        catch (Exception ex)
        {
            ExceptionLogging.LogException(Convert.ToString(ex));
            await errorLogger.Log("usp_Acceptallocation", ex);
            return null;
        }
    }
    public async Task<CallLetterResponseModel?> DownloadCallLetter(int applicationId)
    {
        try
        {
            var param = new DynamicParameters();
            param.Add("p_ApplicationID", applicationId, DbType.Int64);
            var result = await Task.FromResult(dapper.Get<CallLetterResponseModel>("usp_DownloadCallLetter", param, commandType: CommandType.StoredProcedure));
            return result;
        }
        catch (Exception ex)
        {
            ExceptionLogging.LogException(Convert.ToString(ex));
            await errorLogger.Log("usp_DownloadCallLetter", ex);
            return null;
        }
    }
    public async Task<List<ApplicantStatusResponseModel>> GetApplicationStatusAtDesk(int appln)
    {
        try
        {
            var param = new DynamicParameters();
            param.Add("p_applicationId", appln, DbType.Int64);
            var result = await Task.FromResult(dapper.GetAll<ApplicantStatusResponseModel>("usp_ApplicationStatusAtDesk", param, commandType: CommandType.StoredProcedure));
            return result;



        }
        catch (Exception ex)
        {
            ExceptionLogging.LogException(Convert.ToString(ex));
            await errorLogger.Log("usp_ApplicationStatusAtDesk", ex);
            return null;
        }
    }
    public async Task<double?> GetMostRecentCoursepercentage()
    {
        try
        {
            var param = new DynamicParameters();
            param.Add("p_UserId", currentUserService.UserNumericId, DbType.Int64);
            var result = await Task.FromResult(dapper.Get<double?>("usp_GetMostRecentCoursepercentage", param, commandType: CommandType.StoredProcedure));
            return (double?)result;
        }
        catch (Exception ex)
        {
            ExceptionLogging.LogException(Convert.ToString(ex));
            await errorLogger.Log("usp_GetMostRecentCoursepercentage", ex);
            return 0;
        }
    }
    public async Task<PreviewModel> ApplicationDetails(long AppId)
    {
        var response = new PreviewModel();
        try
        {
            var myParams = new DynamicParameters();
            myParams.Add("p_ApplicationId", AppId, DbType.Int64);
            myParams.Add("p_UserId", currentUserService.UserNumericId, DbType.Int64);
            var result = await dapper.MultiResult("usp_Application_Details_View", myParams, commandType: CommandType.StoredProcedure);
            if (result.Count > 0)
            {
                if (result[0] != null)
                {
                    var data = JsonConvert.SerializeObject(result[0].FirstOrDefault());
                    response.AadharData = JsonConvert.DeserializeObject<AadhaarDetailsPreviewModel>(data);
                }
                if (result[1] != null)
                {
                    var data = JsonConvert.SerializeObject(result[1].FirstOrDefault());
                    response.PersonalDetails = JsonConvert.DeserializeObject<PersonalDetailsPreviewModel>(data);
                }
                if (result[2] != null)
                {
                    var data = JsonConvert.SerializeObject(result[2].FirstOrDefault());
                    response.CastDetails = JsonConvert.DeserializeObject<CastDetailsPreviewModel>(data);

                    var data2 = JsonConvert.SerializeObject(result[2].FirstOrDefault());
                    response.DomicileDetails = JsonConvert.DeserializeObject<DomicilePreviewModel>(data2);
                }
                if (result[3] != null)
                {
                    var data = JsonConvert.SerializeObject(result[3].FirstOrDefault());
                    response.PermanentAddress = JsonConvert.DeserializeObject<AddressDetailsPreviewModel>(data);
                }
                if (result[4] != null)
                {
                    var data = JsonConvert.SerializeObject(result[4].FirstOrDefault());
                    response.PresentAddress = JsonConvert.DeserializeObject<AddressDetailsPreviewModel>(data);
                }
                if (result[5] != null)
                {
                    var data = JsonConvert.SerializeObject(result[5].FirstOrDefault());
                    response.ParentAddress = JsonConvert.DeserializeObject<AddressDetails2PreviewModel>(data);
                }
                if (result[6] != null)
                {
                    var data = JsonConvert.SerializeObject(result[6].FirstOrDefault());
                    response.GuardianAddress = JsonConvert.DeserializeObject<AddressDetails2PreviewModel>(data);
                }
                if (result[7] != null)
                {
                    var data = JsonConvert.SerializeObject(result[7].FirstOrDefault());
                    response.OtherDetails = JsonConvert.DeserializeObject<OtherDetailsPreviewModel>(data);
                }
                if (result[8] != null)
                {
                    var data = JsonConvert.SerializeObject(result[8].ToList());
                    response.Preferences = JsonConvert.DeserializeObject<List<HostelPreferencePreviewModel>>(data);
                }
                if (result[9] != null)
                {
                    var data = JsonConvert.SerializeObject(result[9].ToList());
                    response.Documents = JsonConvert.DeserializeObject<List<DocumentPreviewModel>>(data);
                }
                if (result[10] != null)
                {
                    var data = JsonConvert.SerializeObject(result[10].ToList());
                    response.CurrentCourse = JsonConvert.DeserializeObject<List<CurrentCoursePreviewModel>>(data);
                }
                if (result[11] != null)
                {
                    var data = JsonConvert.SerializeObject(result[11].ToList());
                    response.PastCourse = JsonConvert.DeserializeObject<List<PastCoursePreviewModel>>(data);
                }
                if (result[12] != null)
                {
                    var data = JsonConvert.SerializeObject(result[12].ToList());
                    response.PreCurrentCourse = JsonConvert.DeserializeObject<List<PreCurrentCoursePreviewModel>>(data);
                }
                if (result[13] != null)
                {
                    var data = JsonConvert.SerializeObject(result[13].ToList());
                    response.PrePastCourse = JsonConvert.DeserializeObject<List<PrePastCoursePreviewModel>>(data);
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionLogging.LogException(Convert.ToString(ex));
            await errorLogger.Log("usp_ApplicantProfilePreview", ex);
        }
        return response;
    }
    public async Task ApplicationBackup(long ApplicationId)
    {
        try
        {
            var param = new DynamicParameters();
            param.Add("p_ApplicationId", ApplicationId, DbType.Int64);
            await Task.FromResult(dapper.Execute("usp_ApplicantDetailsBackup", param, commandType: CommandType.StoredProcedure));
        }
        catch (Exception ex)
        {
            ExceptionLogging.LogException(Convert.ToString(ex));
            await errorLogger.Log("usp_ApplicantDetailsBackup", ex);
        }
    }

    public async Task<bool> CheckApplicationAlreadyExistForService(int? serviceType)
    {
        try
        {
            var param = new DynamicParameters();
            param.Add("p_UserId", currentUserService.UserNumericId, DbType.Int64);
            param.Add("p_ServiceType", serviceType, DbType.Int32);
             bool rs= await Task.FromResult(dapper.Get<bool>("usp_CheckExistForCurrentYearAndService", param, commandType: CommandType.StoredProcedure));
            return rs;
        }
        catch (Exception ex)
        {
            ExceptionLogging.LogException(Convert.ToString(ex));
            await errorLogger.Log("usp_CheckExistForCurrentYearAndService", ex);
           throw;
            
        }
    }

    public async Task<string> CheckValidationForSwadharApply()
    {
        try
        {
            var param = new DynamicParameters();
            param.Add("p_UserId", currentUserService.UserNumericId, DbType.Int64);
           
            string rs = await Task.FromResult(dapper.Get<string>("usp_GetMostRecentCoursepercentage_V2", param, commandType: CommandType.StoredProcedure));
            return rs;
        }
        catch (Exception ex)
        {
            ExceptionLogging.LogException(Convert.ToString(ex));
            await errorLogger.Log("usp_GetMostRecentCoursepercentage_V2", ex);
            throw;

        }
    }

    public async Task<OfflineApplicationModel?> GetOfflineApplication()
    {
        try
        {
            var param = new DynamicParameters();
            param.Add("p_UserId", currentUserService.UserNumericId, DbType.Int64);
            var result = await Task.FromResult(dapper.Get<OfflineApplicationModel>("usp_GetOfflineApplicant_UserId", param, commandType: CommandType.StoredProcedure));
            return result;
        }
        catch (Exception ex)
        {
            ExceptionLogging.LogException(Convert.ToString(ex));
            await errorLogger.Log("usp_GetOfflineApplicant_UserId", ex);
            return null;
        }
    }


    public async Task<AdmittedApplicationModel?> GetAdmittedApplication()
    {
        try
        {
            var param = new DynamicParameters();
            param.Add("p_UserId", currentUserService.UserNumericId, DbType.Int64);
            var result = await Task.FromResult(dapper.Get<AdmittedApplicationModel>("usp_GetAdmittedApplicant_UserId", param, commandType: CommandType.StoredProcedure));
            return result;
        }
        catch (Exception ex)
        {
            ExceptionLogging.LogException(Convert.ToString(ex));
            await errorLogger.Log("usp_GetAdmittedApplicant_UserId", ex);
            return null;
        }
    }

    public async Task<ApplicationInstallmentModel> GetApplicationInstallmentDetails()
    {
        try
        {
            var param = new DynamicParameters();
            param.Add("p_UserId", currentUserService.UserNumericId, DbType.Int64);
            var result = await Task.FromResult(dapper.Get<ApplicationInstallmentModel>("usp_GetApplicationInstallmentDetails", param, commandType: CommandType.StoredProcedure));
            return result;
        }
        catch (Exception ex)
        {
            ExceptionLogging.LogException(Convert.ToString(ex));
            await errorLogger.Log("usp_GetApplicationInstallmentDetails", ex);
            return null;
        }
    }
}