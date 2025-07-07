using Dapper;
using Helper;
using Model;
using Newtonsoft.Json;
using Repository.Interface;
using Service.Interface;
using System.Data;
using System.Dynamic;

namespace Service.Implementation;

public class CurrentCourseService : ICurrentCourseService
{
    private readonly IDapper dapper;
    private readonly ICurrentUserService currentUserService;
    private readonly IErrorLogger errorLogger;

    public CurrentCourseService(IDapper dapper, ICurrentUserService currentUserService,IErrorLogger errorLogger)
    {
        this.dapper = dapper;
        this.currentUserService = currentUserService;
        this.errorLogger = errorLogger;
    }


    public async Task<string> SaveCurrentCourseDetailsAsync(CurrentCourseDetails model)
    {
        try
        {
            var myparam = new DynamicParameters();
            myparam.Add("_UserId", currentUserService.UserNumericId, DbType.String);
            myparam.Add("_AdmissionYear", model.AdmissionYear, DbType.Int32);
            myparam.Add("_StateID", model.StateID, DbType.Int32);
            myparam.Add("_DistrictID", model.DistrictID, DbType.Int32);
            myparam.Add("_TalukaID", model.TalukaID, DbType.Int32);
            myparam.Add("_CourseTypeID", model.CourseTypeID, DbType.Int32);
            myparam.Add("_CollegeTypeID", model.CollegeTypeID, DbType.Int32);
            myparam.Add("_CourseCategoryId", model.CourseCategoryId, DbType.Int32);
            myparam.Add("_Percentage", model.Percentage, DbType.Decimal);
            myparam.Add("_Result", model.Result, DbType.Int32);
            myparam.Add("_StartYear", model.StartYear, DbType.Int32);
            myparam.Add("_IsCompletedOrContinue", model.IsCompletedOrContinue, DbType.Boolean);
            myparam.Add("_IsCompleted", model.IsCompleted, DbType.Boolean);
            myparam.Add("_GapYears", model.GapYears, DbType.Int32);
            myparam.Add("_QualificationTypeID", model.QualificationTypeID, DbType.Int32);
            myparam.Add("_StreamID", model.StreamID, DbType.Int32);
            myparam.Add("_CourseYrID", model.CourseYrID, DbType.Int32);
            myparam.Add("_AdmissionTypeID", model.AdmissionTypeID, DbType.Int32);
            myparam.Add("_IsAdmissionThroughOpenOrResID", model.IsAdmissionThroughOpenOrResID, DbType.Int32);
            myparam.Add("_EducationMode", model.EducationMode, DbType.Int32);
            myparam.Add("_GapReason", model.GapReason, DbType.String);
            myparam.Add("_IsDeleted", false, DbType.Boolean);
            myparam.Add("_IsActive", true, DbType.Boolean);
            myparam.Add("_CreatedOn", DateTime.Now, DbType.DateTime);
            myparam.Add("_Admissiondate", model.Admissiondate, DbType.DateTime);
            myparam.Add("_CGPA", model.CGPA, DbType.Decimal);
            myparam.Add("_CGPAFileId", model.CGPAFileId, DbType.String);
            myparam.Add("_CollegeName", model.CollegeName, DbType.String);
            var result = await Task.FromResult(dapper.Insert<string>("InsertCurrentCourseDetails", myparam, commandType: System.Data.CommandType.StoredProcedure));
            return result;
        }
        catch (Exception ex)
        {
            ExceptionLogging.LogException(Convert.ToString(ex));
            await errorLogger.Log("InsertCurrentCourseDetails", ex);
            throw;
        }
    }
    public async Task<int> UpdateCurrentCourseDetailsAsync(CurrentCourseDetails model)
    {
        string Query = "update hostelmanagement.applicant_currentcoursedetails set ";
        string setClause = "";
        string completeQuery = "";
        string WhearClause = " WHERE RowID = @RowID";
        var myparam = new DynamicParameters();

        try
        {
            var result = await Task.FromResult(dapper.Execute(completeQuery, myparam, commandType: System.Data.CommandType.Text));
            return 0;
        }
        catch (Exception ex)
        {
            ExceptionLogging.LogException(Convert.ToString(ex));
            throw;
        }
    }
    public async Task<List<CourseDetailsTblViewModel>> GetCourseDetailsByUserId()
    {
        try
        {
            var myparam = new DynamicParameters();
            myparam.Add("_UserID", currentUserService.UserNumericId, DbType.String);
            var result = await Task.FromResult(dapper.GetAll<CourseDetailsTblViewModel>("usp_GetCourseDetailsByUserIDOrRowID", myparam, commandType: System.Data.CommandType.StoredProcedure));
            return result;
        }
        catch (Exception ex)
        {
            ExceptionLogging.LogException(Convert.ToString(ex));
            await errorLogger.Log("usp_GetCourseDetailsByUserIDOrRowID", ex);
            return new List<CourseDetailsTblViewModel>();
        }
    }
    public async Task<string> DeleteCurrentCourseByRowId(long RowId)
    {
        try
        {
            var myparam = new DynamicParameters();
            myparam.Add("_RowId", RowId, DbType.Int64);
            myparam.Add("_UserId", currentUserService.UserNumericId, DbType.Int64);
            myparam.Add("_ConfirmationMessage", dbType: DbType.String, direction: ParameterDirection.Output);
            var result = await Task.FromResult(dapper.ExecuteQuery("usp_deletecrntCourseDetails", myparam, commandType: System.Data.CommandType.StoredProcedure));

            return myparam.Get<string>("_ConfirmationMessage");
        }
        catch (Exception ex)
        {
            ExceptionLogging.LogException(Convert.ToString(ex));
            await errorLogger.Log("usp_deletecrntCourseDetails", ex);
            return "";
        }
    }
    public async Task<ExpandoObject> IsCapRequired(int courseId)
    {
        try
        {
            var myparam = new DynamicParameters();
            myparam.Add("_courseId", courseId, DbType.Int64);
           myparam.Add("_UserId",currentUserService.UserNumericId, DbType.Int64);
            dynamic dynamicObject = new ExpandoObject();
            // var dynamicDict = (IDictionary<string, object>)dynamicObject;

            var result = await dapper.MultiResult("usp_CheckCapRequired", myparam, commandType: CommandType.StoredProcedure);
            if (result.Count > 0)
            {

                if (result[0] != null)
                {
                    var data = JsonConvert.SerializeObject(result[0].FirstOrDefault());
                    if (Convert.ToString(data) != "null")
                    {
                        dynamic jdata = JsonConvert.DeserializeObject<dynamic>(data);
                        dynamicObject.Iscap = (int)jdata.Dept_RequiresCAP;
                    }
                    else
                        dynamicObject.Iscap = 0;

                }
                if (result[1] != null)
                {
                    var data = JsonConvert.SerializeObject(result[1].FirstOrDefault());
                    if (Convert.ToString(data) != "null")
                    {
                        dynamic jdata = JsonConvert.DeserializeObject<dynamic>(data);

                        // Access the 'is_professional' value
                        dynamicObject.Isprofessional = (int)jdata.is_professional;

                        //  dynamicObject.Isprofessional = jsonObject["is_professional"].Value<int>();
                    }
                    else
                        dynamicObject.Isprofessional = 1;

                }
                if (result[2] != null)
                {
                    var data = JsonConvert.SerializeObject(result[2].FirstOrDefault());
                    if (Convert.ToString(data) != "null")
                    {
                        dynamic jdata = JsonConvert.DeserializeObject<dynamic>(data);

                        // Access the 'is_professional' value
                        dynamicObject.TypeOfCourse = (int)jdata.TypeOfCourse;

                        //  dynamicObject.Isprofessional = jsonObject["is_professional"].Value<int>();
                    }
                    else
                        dynamicObject.TypeOfCourse = 0;

                }
            }

            return dynamicObject;
        }
        catch (Exception ex)
        {
            ExceptionLogging.LogException(Convert.ToString(ex));
            await errorLogger.Log("usp_CheckCapRequired", ex);
            return new ExpandoObject();
        }
    }
    public async Task<string> CalculateYearGap(int courseYrId, string selectedYear)
    {
        try
        {
            var myparam = new DynamicParameters();
            myparam.Add("_UserId", currentUserService.UserNumericId, DbType.Int64);
            myparam.Add("This_Year", selectedYear, DbType.Int64);
            myparam.Add("CurrentAcademicYear", courseYrId, DbType.Int64);
           var result = await Task.FromResult(dapper.Get<string>("Usp_Gap_Calculation", myparam, commandType: System.Data.CommandType.StoredProcedure));
            return result;
        }
        catch (Exception ex)
        {
            ExceptionLogging.LogException(Convert.ToString(ex));
            await errorLogger.Log("Usp_Gap_Calculation", ex);
            return string.Empty;
        }
    }
    public async Task<string> CheckIsDatainYearOrder(int CourseId, int YearStudy)
    {
        try
        {
            var myparam = new DynamicParameters();
            myparam.Add("_UserId", currentUserService.UserNumericId, DbType.Int64);
            myparam.Add("YearStudy", YearStudy, DbType.Int64);
            myparam.Add("CourseID", CourseId, DbType.Int64);
            var result = await Task.FromResult(dapper.Get<string>("usp_IsValidYearSelected", myparam, commandType: CommandType.StoredProcedure));
            return result;
        }
        catch (Exception ex)
        {
            ExceptionLogging.LogException(Convert.ToString(ex));
            await errorLogger.Log("usp_IsValidYearSelected", ex);
            return string.Empty;
        }
    }
    public async Task<List<personalInfo>> GetPersonalInfoByUserId()
    {
        try
        {
            var myparam = new DynamicParameters();
            myparam.Add("_UserId", currentUserService.UserNumericId, DbType.Int64);
            var result = await Task.FromResult(dapper.GetAll<personalInfo>("usp_GetPersonaldetailsForCurrent", myparam, commandType: CommandType.StoredProcedure));
            return result;
        }
        catch (Exception ex)
        {
            ExceptionLogging.LogException(Convert.ToString(ex));
            await errorLogger.Log("usp_GetPersonaldetailsForCurrent", ex);
            return new List<personalInfo>();
        }
    }
    public async Task<List<personalInfo>> persona_reservedCategory()
    {
        try
        {
            var myparam = new DynamicParameters();
            myparam.Add("_UserId", currentUserService.UserNumericId, DbType.Int64);
            var result = await Task.FromResult(dapper.GetAll<personalInfo>("usp_GetCategoryByUserId", myparam, commandType:CommandType.StoredProcedure));
            return result;
        }
        catch (Exception ex)
        {
            ExceptionLogging.LogException(Convert.ToString(ex));
            await errorLogger.Log("usp_GetCategoryByUserId", ex);
            return new List<personalInfo>();
        }
    }
    public async Task<List<SelectListModel>> Getqualificationtype(string caller)
    {
        try
        {
            var myparam = new DynamicParameters();
            myparam.Add("_UserId", currentUserService.UserNumericId, DbType.Int64);
            myparam.Add("caller", caller, DbType.String);
            var result = await Task.FromResult(dapper.GetAll<SelectListModel>("usp_GetQualificationType", myparam, commandType: System.Data.CommandType.StoredProcedure));
            return result;
        }
        catch (Exception ex)
        {
            ExceptionLogging.LogException(Convert.ToString(ex));
            await errorLogger.Log("usp_GetQualificationType", ex);
            return new List<SelectListModel>();
        }
    }
    public async Task<string> GetCurrentCourseStartYear()
    {
        try
        {
            var myparam = new DynamicParameters();
            myparam.Add("_UserId", currentUserService.UserNumericId, DbType.Int64);
            var result = await Task.FromResult(dapper.Get<string>("GetCurrentCourseStartYear", myparam, commandType: System.Data.CommandType.StoredProcedure));
            return result;
        }
        catch (Exception ex)
        {
            ExceptionLogging.LogException(Convert.ToString(ex));
            await errorLogger.Log("GetCurrentCourseStartYear", ex);
            return string.Empty;
        }
    }
    public async Task<List<SelectListModel2>> PreSchools(string stateID, string districtID, string talukaID)
    {
        try
        {
            var myparam = new DynamicParameters();
            myparam.Add("p_state_code_census", stateID, DbType.Int64);
            myparam.Add("p_district_code_census", districtID, DbType.Int64);
            myparam.Add("p_taluka_code_census", talukaID, DbType.Int64);
            var result = await Task.FromResult(dapper.GetAll<SelectListModel2>("usp_FilterPreSchoolData", myparam, commandType: System.Data.CommandType.StoredProcedure));
            return result;
        }
        catch (Exception ex)
        {
            ExceptionLogging.LogException(Convert.ToString(ex));
            await errorLogger.Log("usp_FilterPreSchoolData", ex);
            return new List<SelectListModel2>();
        }
    }

    public async Task<List<SelectListModel>> PreStandard(string caller)
    {
        try
        {
            var myparam = new DynamicParameters();
            myparam.Add("p_caller", caller, DbType.String);
            var result = await Task.FromResult(dapper.GetAll<SelectListModel>("usp_GetPreStandard", myparam, commandType: System.Data.CommandType.StoredProcedure));
            return result;
        }
        catch (Exception ex)
        {
            ExceptionLogging.LogException(Convert.ToString(ex));
            await errorLogger.Log("usp_GetPreStandard", ex);
            return new List<SelectListModel>();
        }
    }
    public async Task<int> SaveApplicantPreSchoolRecord(ApplicantPreSchoolRecord model)
    {
        try
        {
            var myparam = new DynamicParameters();
            myparam.Add("p_UserId", currentUserService.UserNumericId, DbType.String);
            myparam.Add("p_stateID", model.StateID, DbType.Int32);
            myparam.Add("p_districtID", model.DistrictID, DbType.Int32);
            myparam.Add("p_talukaID", model.TalukaID, DbType.Int32);
            myparam.Add("p_schoolName", model.SchoolName, DbType.String);
            myparam.Add("p_schoolUDISE", model.SchoolUDISE, DbType.String);
            myparam.Add("p_standard", model.Standard, DbType.Int32);
            myparam.Add("p_result", model.Result, DbType.Int32);
            myparam.Add("p_prevAttendence", model.PrevAttendence, DbType.Decimal);
            myparam.Add("p_prevPercentage", model.PrevPercentage, DbType.Decimal);
            myparam.Add("p_prevRank", model.PrevRank, DbType.Int32);
            myparam.Add("p_isGap", model.IsGap, DbType.Boolean);
            myparam.Add("p_IsCompleted", model.IsCompleted, DbType.Boolean);
            myparam.Add("p_admissionDate", model.AdmissionDate, DbType.Date);
            myparam.Add("p_GapReason", model.GapReason, DbType.String);
         
            var result = await Task.FromResult(dapper.Insert<int>("Insert_applicant_PreSchoolRecords", myparam, commandType: System.Data.CommandType.StoredProcedure));
            return result;
        }
        catch (Exception ex)
        {
            ExceptionLogging.LogException(Convert.ToString(ex));
            await errorLogger.Log("Insert_applicant_PreSchoolRecords", ex);
            return 0;
        }
    }
   
    public async Task<int> DeletePreCourseRecord(long Id)
    {
        try
        {
            var myparam = new DynamicParameters();
            myparam.Add("_Id", Id, DbType.Int64);
            myparam.Add("_UserId", currentUserService.UserNumericId, DbType.Int64);
            var result = await Task.FromResult(dapper.ExecuteQuery("usp_deletePreSchool", myparam, commandType: System.Data.CommandType.StoredProcedure));
            return result;
        }
        catch (Exception ex)
        {
            ExceptionLogging.LogException(Convert.ToString(ex));
            await errorLogger.Log("usp_deletePreSchool", ex);
            return 0;
        }
    }

    public async Task<List<ApplicantPreSchoolRecordView>> GetPreDetailsByUserId()
    {
        try
        {
            var myparam = new DynamicParameters();
            myparam.Add("_UserID", currentUserService.UserNumericId, DbType.String);
            var result = await Task.FromResult(dapper.GetAll<ApplicantPreSchoolRecordView>("usp_GetPreCourseDetails", myparam, commandType: CommandType.StoredProcedure));
            return result;
        }
        catch (Exception ex)
        {
            ExceptionLogging.LogException(Convert.ToString(ex));
            await errorLogger.Log("usp_GetPreCourseDetails", ex);
            return new List<ApplicantPreSchoolRecordView>();
        }
    }
  
    public async Task<List<SelectListModel>> GetCourseStatus(string _preOrPot, int _standardId)
    {
        try
        {
            var myparam = new DynamicParameters();
            myparam.Add("_UserID", currentUserService.UserNumericId, DbType.String);
            myparam.Add("_preOrPot", _preOrPot, DbType.String);
            myparam.Add("_standardId", _standardId, DbType.Int32);
            var result = await Task.FromResult(dapper.GetAll<SelectListModel>("usp_GetCourseStatusByUserId", myparam, commandType: System.Data.CommandType.StoredProcedure));
            return result;
        }
        catch (Exception ex)
        {
            ExceptionLogging.LogException(Convert.ToString(ex));
            await errorLogger.Log("usp_GetCourseStatusByUserId", ex);
            throw;
        }
    }

    public async Task<List<SelectListModel>> GetCourseStatusYearByUserId(int yearId)
    {
        try
        {
            var myparam = new DynamicParameters();
            myparam.Add("_YearId", yearId, DbType.Int32);
            myparam.Add("_UserdId", currentUserService.UserNumericId, DbType.Int32);


            var result = await Task.FromResult(dapper.GetAll<SelectListModel>("usp_GetCourseStatusYearByUserId", myparam, commandType: System.Data.CommandType.StoredProcedure));
            return result;
        }
        catch (Exception ex)
        {
            ExceptionLogging.LogException(Convert.ToString(ex));
            await errorLogger.Log("usp_GetCourseStatusYearByUserId", ex);
            throw;
        }
    }

    public async Task<List<SelectListModel>> GetCurrentQualificationYear(int courseId, int? langID = 1)
    {
        try
        {
            var myparam = new DynamicParameters();
            myparam.Add("_intID", courseId, DbType.Int32);
            myparam.Add("_LangId", langID, DbType.Int32);
            myparam.Add("_UserID", currentUserService.UserNumericId, DbType.Int32);


            var result = await Task.FromResult(dapper.GetAll<SelectListModel>("usp_GetCurrentQualificationYearByUserId", myparam, commandType: System.Data.CommandType.StoredProcedure));
            return result;
        }
        catch (Exception ex)
        {
            ExceptionLogging.LogException(Convert.ToString(ex));
            await errorLogger.Log("usp_GetCurrentQualificationYearByUserId", ex);
            throw;
        }
    }
    public async Task<bool> IsNewApplicant()
    {
        try
        {
            bool flag = false;
            var myparam = new DynamicParameters();
            myparam.Add("_UserID", currentUserService.UserNumericId, DbType.Int32);
           var result = await Task.FromResult(dapper.Get<string>("usp_IsNewApplicant", myparam, commandType: System.Data.CommandType.StoredProcedure));
            if(result != null && result !="") {
                flag = result == "1" ? true : false;
            }
            return flag;
        }
        catch (Exception ex)
        {
            ExceptionLogging.LogException(Convert.ToString(ex));
            throw;
        }
    }
    public async Task<bool> CheckIsCurrentAvailable()
    {
        try
        {
           
            var myparam = new DynamicParameters();
            myparam.Add("_UserID", currentUserService.UserNumericId, DbType.Int32);
            var result = await Task.FromResult(dapper.Get<bool>("usp_CheckIsCurrentAvailable", myparam, commandType: System.Data.CommandType.StoredProcedure));
           
            return result;
        }
        catch (Exception ex)
        {
            ExceptionLogging.LogException(Convert.ToString(ex));
            await errorLogger.Log("usp_CheckIsCurrentAvailable", ex);
            throw;
        }
    }
    public async Task<string> GetAadharAndCheckShouldFetchRecordFromPost(string Caller)
    {
        try
        {

            var myparam = new DynamicParameters();
            myparam.Add("_UserID", currentUserService.UserNumericId, DbType.Int32); 
            myparam.Add("Caller", Caller, DbType.String);
            var result = await Task.FromResult(dapper.Get<string>("usp_GetAadharNumberByUserId", myparam, commandType: System.Data.CommandType.StoredProcedure));

            return result;
        }
        catch (Exception ex)
        {
            ExceptionLogging.LogException(Convert.ToString(ex));
            await errorLogger.Log("usp_GetAadharNumberByUserId", ex);
            throw;
        }
    }

    public async Task<string> GetCourseDuration(int courseId)
    {
        var myparam = new DynamicParameters();
        myparam.Add("p_courseId", courseId, DbType.Int32);
       
        var result = await Task.FromResult(dapper.Get<string>("usp_GetCourseduration", myparam, commandType: System.Data.CommandType.StoredProcedure));
        return result;
    }
    public async Task<List<SelectListModel>> DistList(int? state, int? lang = 1)
    {
        try
        {   //usp_GetDistrictList
            var myparam = new DynamicParameters();
            myparam.Add("StateCode", state, DbType.Int32);
            myparam.Add("Lang", lang, DbType.Int32);
            myparam.Add("p_UserId", currentUserService.UserNumericId, DbType.Int32);
            var result = await Task.FromResult(dapper.GetAll<SelectListModel>("usp_GetDistrictListForCurrent", myparam, commandType: System.Data.CommandType.StoredProcedure));
            return result;
        }
        catch (Exception ex)
        {
            ExceptionLogging.LogException(Convert.ToString(ex));
            await errorLogger.Log("usp_GetDistrictListForCurrent", ex);
            return new List<SelectListModel>();
        }
    }
    public async Task<List<SelectListModel>> TalukaList(int? distid, int? lang = 1)
    {
        try
        {   //usp_GetTalukaList
            var myparam = new DynamicParameters();
            myparam.Add("_Lang", lang, DbType.Int32);
            myparam.Add("_DistrictCode", distid, DbType.Int32);
            myparam.Add("p_UserId", currentUserService.UserNumericId, DbType.Int32);
            var result = await Task.FromResult(dapper.GetAll<SelectListModel>("usp_GetTalukaListForCurrent", myparam, commandType: System.Data.CommandType.StoredProcedure));
            return result;
        }
        catch (Exception ex)
        {
            ExceptionLogging.LogException(Convert.ToString(ex));
            await errorLogger.Log("usp_GetTalukaListForCurrent", ex);
            return new List<SelectListModel>();
        }
    }
}
