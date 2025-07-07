using Dapper;
using Helper;
using Model;
using Repository.Interface;
using Service.Interface;
using System.Collections.Generic;
using System.Data;

namespace Service.Implementation;

public class LoginDetailsService : ILoginDetailsService
{
    private readonly IDapper dapper;
    private readonly ICurrentUserService currentUserService;
    private readonly IErrorLogger errorLogger;
    public LoginDetailsService(IDapper dapper,ICurrentUserService currentUserService,IErrorLogger errorLogger)
    {
        this.dapper = dapper;
        this.currentUserService = currentUserService;
        this.errorLogger = errorLogger;
    }
    public async Task<WebServiceUserModel> GetLoginDetails(long Userid)
    {
        try
        {
            var myParams = new DynamicParameters();
            myParams.Add("p_UserID", Userid, DbType.Int64);            
            var result = await Task.FromResult(dapper.Get<WebServiceUserModel>("usp_GetLoginDetailsByUserId", myParams, commandType: CommandType.StoredProcedure));
            return result;
        }
        catch (Exception ex)
        {
            // ExceptionLogging.LogException(Convert.ToString(ex));
            await errorLogger.Log("usp_GetLoginDetailsByUserId",ex);
            return new WebServiceUserModel();
        }
    }
    public async Task<WebServiceUserModel> GetLoginDetails(string aadaharReferenceNo)
    {
        try
        {
            var myParams = new DynamicParameters();
            myParams.Add("p_AadharReferenceNo", aadaharReferenceNo, DbType.Int64);
            var result = await Task.FromResult(dapper.Get<WebServiceUserModel>("usp_GetLoginDetailsByAadhar", myParams, commandType: CommandType.StoredProcedure));
            return result;
        }
        catch (Exception ex)
        {
            // ExceptionLogging.LogException(Convert.ToString(ex));
            await errorLogger.Log("usp_GetLoginDetailsByAadhar", ex);
            return new WebServiceUserModel();
        }
    }
    public async Task<WebServiceUserModel> SaveLoginDetails(WebServiceUserModel model)
    {
		try
		{
            var myParams = new DynamicParameters();
            myParams.Add("p_UserID", model.UserID, DbType.String);
            myParams.Add("p_UserName", model.UserName, DbType.String);
            myParams.Add("p_Password", model.Password, DbType.String);
            myParams.Add("p_EmailId", model.EmailID, DbType.String);
            myParams.Add("p_MobileNo", model.MobileNo, DbType.String);
            myParams.Add("p_FullName", model.FullName, DbType.String);
            myParams.Add("p_FullNameInMarathi", model.FullNameInMarathi, DbType.String);
            myParams.Add("p_Age", "0", DbType.Int32);
            myParams.Add("p_Gender", model.Gender, DbType.String);
            myParams.Add("p_DateofBirth", model.DateofBirth, DbType.String);
            myParams.Add("p_UserIdentity", model.UserIdentity,  DbType.String);
            myParams.Add("IsMobileVerified", model.IsMobileVerified,  DbType.Boolean);
            myParams.Add("IsEmailVerified", model.IsEmailVerified,  DbType.Boolean);
            myParams.Add("Domain", model.Domain,  DbType.String);
            var result = await Task.FromResult(dapper.Insert<WebServiceUserModel>("usp_InsertLoginDetails", myParams, commandType: CommandType.StoredProcedure));
            return result;
        }
		catch (Exception ex)
		{
            // ExceptionLogging.LogException(Convert.ToString(ex));
            await errorLogger.Log("usp_InsertLoginDetails", ex);
            return new WebServiceUserModel();
		}
    }

    public async Task<string> UpdateOfflineUIDReference(string UID)
    {
        try
        {
            var myParams = new DynamicParameters();
            myParams.Add("p_UserID", currentUserService.UserNumericId, DbType.String);
            myParams.Add("UID", UID, DbType.String);

            var result = await Task.FromResult(dapper.Insert<string>("usp_UpdateOfflineUIDReference", myParams, commandType: CommandType.StoredProcedure));
            return result;
        }
        catch (Exception ex)
        {
            // ExceptionLogging.LogException(Convert.ToString(ex));
            await errorLogger.Log("usp_UpdateOfflineUIDReference", ex);
            return "";
        }
    }

    
    public async Task<VerifiedStatusModel> VerifyStatus()
    {
        try
        {

            var myParams = new DynamicParameters();
            myParams.Add("p_UserNumericId", currentUserService.UserNumericId, DbType.Int64);
            var result = await Task.FromResult(dapper.Get<VerifiedStatusModel>("usp_GetVerifiedStatus", myParams, CommandType.StoredProcedure));
            return result;
        }
        catch (Exception ex)
        {
            // ExceptionLogging.LogException(Convert.ToString(ex));
            await errorLogger.Log("usp_GetVerifiedStatus", ex);
            return new VerifiedStatusModel();
        }
    }

    public async Task<string> GetAadharreferenceNumber()
    {
        try
        {

            var myParams = new DynamicParameters();
            myParams.Add("_UserID", currentUserService.UserNumericId, DbType.Int64);
            var result = await Task.FromResult(dapper.Get<string>("usp_GetAadharreferenceNumber", myParams, CommandType.StoredProcedure));
            return result;
        }
        catch (Exception ex)
        {
            // ExceptionLogging.LogException(Convert.ToString(ex));
            await errorLogger.Log("usp_GetAadharreferenceNumber", ex);
            return  "";
        }
    }
    
    public async Task<UIDResponseModel> UpsertAadhaarAsync(UIDResponseModel model)
    {
        try
        {
            //var birthDate = Utility.ConvertToDate(model.DateOfBirth);           
			var myParams = new DynamicParameters();
            myParams.Add("p_UserId", currentUserService.UserNumericId, DbType.Int64);
            myParams.Add("p_UIDNo", model.UIDNo, DbType.String);
            myParams.Add("p_UIDReference", model.ReferenceNo, DbType.String);
            myParams.Add("p_ApplicantName", model.ApplicantName, DbType.String);
            myParams.Add("p_ApplicantNameLL", model.ApplicantName_LL, DbType.String);
            myParams.Add("p_DateOfBirth", model.DateOfBirth, DbType.String);
            myParams.Add("p_Gender", model.Gender, DbType.String);
            myParams.Add("p_Mobile", model.MobileNo, DbType.String);
            myParams.Add("p_State", model.StateName, DbType.String);
            myParams.Add("p_District", model.DistrictName, DbType.String);
            myParams.Add("p_Taluka", model.TalukaName, DbType.String);
            myParams.Add("p_Address", model.Address, DbType.String);
            myParams.Add("p_Pincode", model.Pincode, DbType.String);
            myParams.Add("p_AadhaarImage", model.ApplicantImage_string, DbType.String);
            myParams.Add("p_CreatedBy", currentUserService.UserId, DbType.String);
            var result = await Task.FromResult(dapper.Insert<UIDResponseModel>("usp_SaveAadhaarDetails", myParams, commandType: CommandType.StoredProcedure));
            return result;
        }
        catch (Exception ex)
        {
            // ExceptionLogging.LogException(Convert.ToString(ex));
            await errorLogger.Log("usp_SaveAadhaarDetails", ex);
            return new UIDResponseModel();
        }
    }
    public async Task<UIDResponseModel> GetRegisterDetails()
    {
        try
        {
            var myParams = new DynamicParameters();
            myParams.Add("p_UserId", currentUserService.UserNumericId, DbType.Int64);            
            var result = await Task.FromResult(dapper.Insert<UIDResponseModel>("usp_GetRegisterDetails", myParams, commandType: CommandType.StoredProcedure));
            return result;
        }
        catch (Exception ex)
        {
            // ExceptionLogging.LogException(Convert.ToString(ex));
            await errorLogger.Log("usp_GetRegisterDetails", ex);
            return new UIDResponseModel();
        }
    }
    public async Task<WebServiceUserModel> SaveRegisterDetails(ProfileVerifyModel model)
    {
        try
        {
            var myParams = new DynamicParameters();
            myParams.Add("p_UserId", currentUserService.UserNumericId, DbType.Int64);            
            myParams.Add("p_Mobile", model.MobileNo, DbType.String);        
            myParams.Add("p_Email", model.EmailID, DbType.String);
            var result = await Task.FromResult(dapper.Insert<WebServiceUserModel>("usp_SaveRegisterDetails", myParams, commandType: CommandType.StoredProcedure));
            return result;
        }
        catch (Exception ex)
        {
            // ExceptionLogging.LogException(Convert.ToString(ex));
            await errorLogger.Log("usp_SaveRegisterDetails", ex);
            return new WebServiceUserModel();
        }
    }
    public async Task<bool> CheckEmailExist(string email)
    {
        try
        {
            var myParams = new DynamicParameters();
            myParams.Add("p_UserId", currentUserService.UserNumericId, DbType.Int64);
            myParams.Add("p_Email", email, DbType.String);
            myParams.Add("Result", dbType: DbType.Byte, direction: ParameterDirection.Output);
            await Task.FromResult(dapper.Execute("usp_CheckEmailExists", myParams, CommandType.StoredProcedure));
            bool result = myParams.Get<byte>("Result") == 1;
            return result;
        }
        catch (Exception ex)
        {
            // ExceptionLogging.LogException(Convert.ToString(ex));
            await errorLogger.Log("usp_SaveRegisterDetails", ex);
            return false;            
        }
    }
    public async Task<bool> CheckMobileExist(string mobile)
    {
        try
        {
            var myParams = new DynamicParameters();
            myParams.Add("p_UserId", currentUserService.UserNumericId, DbType.Int64);
            myParams.Add("p_Mobile", mobile, DbType.String);
            myParams.Add("Result", dbType: DbType.Byte, direction: ParameterDirection.Output);
            await Task.FromResult(dapper.Execute("usp_CheckMobileExists", myParams, CommandType.StoredProcedure));
            bool result = myParams.Get<byte>("Result") == 1;
            return result;
        }
        catch (Exception ex)
        {
            // ExceptionLogging.LogException(Convert.ToString(ex));
            await errorLogger.Log("usp_CheckMobileExists", ex);
            return false;           
        }
    }
    public async Task<List<SelectListModel>> HostelGenderWise(int dist, int? taluka)
    {
        try
        {
            var param = new DynamicParameters();
            param.Add("Dist", dist, DbType.Int32);
            param.Add("Taluka", taluka, DbType.Int32);
            param.Add("p_UserId", currentUserService.UserNumericId, DbType.Int64);
            var result = await Task.FromResult(dapper.GetAll<SelectListModel>("usp_GetHostelMasterListGenderWise", param, commandType: CommandType.StoredProcedure));
            return result;
        }
        catch (Exception ex)
        {
            // ExceptionLogging.LogException(Convert.ToString(ex));
            await errorLogger.Log("usp_CancelApplication", ex);
            return null;
        }
    }

    public async Task<string> GetUIDNo()
    {
        try
        {
            var param = new DynamicParameters();
            param.Add("p_UserId", 23429, DbType.Int64);

            var result = await Task.FromResult(dapper.Get<string>("usp_GetEncryptedAadharNumber", param, commandType: CommandType.StoredProcedure));
            return result;
        }
        catch (Exception ex)
        {
            // ExceptionLogging.LogException(Convert.ToString(ex));
            await errorLogger.Log("usp_GetEncryptedAadharNumber", ex);
            return null;
        }
    }
}