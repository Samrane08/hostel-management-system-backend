using Dapper;
using Helper;
using Model;
using Newtonsoft.Json;
using Repository.Interface;
using Service.Interface;
using System.Data;

namespace Service.Implementation;

public class PastCourseService : IPastCourseService
{
    private readonly IDapper dapper;
    private readonly ICurrentUserService currentUserService;
    private readonly IErrorLogger errorLogger;

    public PastCourseService(IDapper dapper, ICurrentUserService currentUserService, IErrorLogger errorLogger)
    {
        this.dapper = dapper;
        this.currentUserService = currentUserService;
        this.errorLogger = errorLogger;
    }
    public async Task<int> SaveAsync(EducationDetails model)
    {
        try
        {
            var data=JsonConvert.SerializeObject(model);
            var myparam = new DynamicParameters();
            myparam.Add("p_UserID", currentUserService.UserNumericId, DbType.String);
            myparam.Add("p_QualificationTypeID", Convert.ToInt32(model.QualificationLevel), DbType.Int32);
            myparam.Add("p_DegreeNameID", Convert.ToInt32(model.StreamID), DbType.Int32);
            myparam.Add("p_IsCompletedOrPursuing", true, DbType.Boolean);
            myparam.Add("p_StateID", Convert.ToInt32(model.StateID), DbType.Int32);
            myparam.Add("p_BoardUniversityID", model.BoardUniversity != "" ? Convert.ToInt32(model.BoardUniversity) : 0, DbType.Int32);
            myparam.Add("p_Result", model.Result, DbType.String);
            myparam.Add("p_Attempts", Convert.ToInt32(model.Attempts), DbType.Int32);
            myparam.Add("p_Percentage", Convert.ToDecimal(model.Percentage), DbType.Decimal);
            myparam.Add("p_ModeID", Convert.ToInt32(model.Mode), DbType.Int32);
            if (model.IsGap == "Yes")
                myparam.Add("p_IsGap", true, DbType.Boolean);
            else
                myparam.Add("p_IsGap", false, DbType.Boolean);
            myparam.Add("p_GapYears", model.GapYears != "" ? Convert.ToInt32(model.GapYears) : 0, DbType.Int32);
            myparam.Add("p_CourseId", model.CourseID != "" ? Convert.ToInt32(model.CourseID) : 0, DbType.Int32);
            myparam.Add("p_PassingYr", model.PassingYear, DbType.String);
            myparam.Add("p_AdmissionYr", model.AdmissionYear, DbType.Int32);
            myparam.Add("p_DistrictID", model.DistrictID, DbType.Int32);
            myparam.Add("p_TalukaID", model.TalukaID, DbType.Int32);
            myparam.Add("p_CollegeID", model.CollegeID != "" ? Convert.ToInt32(model.CollegeID) : 0, DbType.Int32);
            myparam.Add("p_CollegeSchoolName", model.CollegeName, DbType.String);
            myparam.Add("p_CourseName", model.CourseName, DbType.String);
            myparam.Add("p_UniversityName", model.UniversityName, DbType.String);
            myparam.Add("p_IsActive", true, DbType.Boolean);
            myparam.Add("p_CreatedDate", DateTime.Now, DbType.DateTime);
            myparam.Add("p_CGPA", model.CGPA, DbType.Decimal);
            myparam.Add("p_CGPAFileId", model.CGPAFileId, DbType.String);
             var result = await Task.FromResult(dapper.Insert<int>("usp_InsertApplicantEducationDetails", myparam, commandType: System.Data.CommandType.StoredProcedure));
            return result;
        }
        catch (Exception ex)
        {
            // ExceptionLogging.LogException(Convert.ToString(ex));
            await errorLogger.Log("usp_InsertApplicantEducationDetails", ex);
            return 0;
        }
    }
    public async Task<int> UpdateAsync(ApplicantEducationDetails model)
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
            // ExceptionLogging.LogException(Convert.ToString(ex));
            await errorLogger.Log("applicant_currentcoursedetails update", ex);
            return 0;
        }
    }
    public async Task<List<CourseDetailsTblViewModel>> GetAsync()
    {
        try
        {
            var myparam = new DynamicParameters();
            myparam.Add("_UserID", currentUserService.UserNumericId, DbType.String);
            var result = await Task.FromResult(dapper.GetAll<CourseDetailsTblViewModel>("usp_GetEducationDetails", myparam, commandType: System.Data.CommandType.StoredProcedure));
            return result;
        }
        catch (Exception ex)
        {
            // ExceptionLogging.LogException(Convert.ToString(ex));
            await errorLogger.Log("usp_GetEducationDetails", ex);
            return new List<CourseDetailsTblViewModel>();
        }
    }   
    public async Task<int> DeleteAsync(long RowId)
    {
        try
        {
            var myparam = new DynamicParameters();
            myparam.Add("_RowId", RowId, DbType.Int64);
            myparam.Add("_UserId", currentUserService.UserNumericId, DbType.Int64);
            var result = await Task.FromResult(dapper.ExecuteQuery("usp_deletepastcourse", myparam, commandType: System.Data.CommandType.StoredProcedure));
            return result;
        }
        catch (Exception ex)
        {
            // ExceptionLogging.LogException(Convert.ToString(ex));
            await errorLogger.Log("usp_deletepastcourse", ex);
            return 0;
        }
    }
    public async Task<int> SavePrePast(ApplicantPreschoolRecordPast model)
    {
        try
        {
            var myparam = new DynamicParameters();
            myparam.Add("p_UserId", currentUserService.UserNumericId, DbType.String);
            myparam.Add("p_stateID", model.StateId, DbType.Int32);
            myparam.Add("p_districtID", model.DistrictId, DbType.Int32);
            myparam.Add("p_talukaID", model.TalukaId, DbType.Int32);
            myparam.Add("p_schoolName", model.SchoolName, DbType.String);
            myparam.Add("p_schoolUDISE", model.SchoolUdise, DbType.String);
            myparam.Add("p_standard", model.Standard, DbType.Int32);
            myparam.Add("p_result", model.Result, DbType.Int32);
            myparam.Add("p_percentage", model.Percentage, DbType.Decimal);
            myparam.Add("p_Rank", model.Rank, DbType.Decimal);
            myparam.Add("p_isGap", model.IsGap, DbType.Boolean);
            myparam.Add("p_IsCompleted", model.IsCompleted, DbType.Boolean);
            myparam.Add("p_StartYear", model.StartYear, DbType.Int32);
            myparam.Add("p_PassingYear", model.PassingYear, DbType.Int32);
            myparam.Add("p_PassingYear", model.PassingYear, DbType.Int32);
            myparam.Add("p_GapReason", model.GapReason, DbType.String);
         
            var result = await Task.FromResult(dapper.Insert<int>("usp_InsertPreschoolRecordPast", myparam, commandType: System.Data.CommandType.StoredProcedure));
            return result;
        }
        catch (Exception ex)
        {
            // ExceptionLogging.LogException(Convert.ToString(ex));
            await errorLogger.Log("usp_InsertPreschoolRecordPast", ex);
            return 0;
        }
    }
    public async Task<int> DeletePrePast(long Id)
    {
        try
        {
            var myparam = new DynamicParameters();
            myparam.Add("_Id", Id, DbType.Int64);
            myparam.Add("_UserId", currentUserService.UserNumericId, DbType.Int64);
            var result = await Task.FromResult(dapper.ExecuteQuery("usp_deletePrepastSchool", myparam, commandType: System.Data.CommandType.StoredProcedure));
            return result;
        }
        catch (Exception ex)
        {
            // ExceptionLogging.LogException(Convert.ToString(ex));
            await errorLogger.Log("usp_deletePrepastSchool", ex);
            return 0;
        }
    }
    public async Task<string> CheckIsPreOrPost()
    {
        try
        {
            var myparam = new DynamicParameters();
            myparam.Add("_UserId", currentUserService.UserNumericId, DbType.Int64);
            var result = await Task.FromResult(dapper.Get<string>("usp_IsPostOrPreRender", myparam, commandType: System.Data.CommandType.StoredProcedure));
            return result;
        }
        catch (Exception ex)
        {
            // ExceptionLogging.LogException(Convert.ToString(ex));
            await errorLogger.Log("usp_IsPostOrPreRender", ex);
            return string.Empty;
        }
    }
    public async Task<List<ApplicantPreSchoolRecordView>> GetPrePast()
    {
        try
        {
            var myparam = new DynamicParameters();
            myparam.Add("_UserID", currentUserService.UserNumericId, DbType.String);
            var result = await Task.FromResult(dapper.GetAll<ApplicantPreSchoolRecordView>("usp_GetPrepastCourseDetails", myparam, commandType: System.Data.CommandType.StoredProcedure));
            return result;
        }
        catch (Exception ex)
        {
            // ExceptionLogging.LogException(Convert.ToString(ex));
            await errorLogger.Log("usp_GetPrepastCourseDetails", ex);
            return new List<ApplicantPreSchoolRecordView>();
        }
    }
    public async Task<string> GetMininumEntryYear()
    {
        try
        {
            var myparam = new DynamicParameters();
            myparam.Add("_UserId", currentUserService.UserNumericId, DbType.Int64);
            var result = await Task.FromResult(dapper.Get<string>("usp_GetMinimumyearEntryInPreSchool", myparam, commandType: System.Data.CommandType.StoredProcedure));
            return result;
        }
        catch (Exception ex)
        {
            // ExceptionLogging.LogException(Convert.ToString(ex));
            await errorLogger.Log("usp_GetMinimumyearEntryInPreSchool", ex);
            return string.Empty;
        }
    }
}
