using Dapper;
using Helper;
using Model;
using Repository.Interface;
using Service.Interface;
using System.Data;

namespace Service.Implementation
{
    public class AccountService: IAccountService
    {
        private readonly IDapperResolver dapperresolverInstance;
        private readonly ICurrentUserService currentUserService;
        private readonly IErrorLogger errorLogger;
        public AccountService(IDapperResolver dapperresolverInstance, ICurrentUserService currentUserService, IErrorLogger errorLogger)
        {
            this.dapperresolverInstance = dapperresolverInstance;
            this.currentUserService = currentUserService;
            this.errorLogger = errorLogger;
        }
        public async Task<HostelProfileModel?> GetHostelProfile(string UserId,int? deptId)
        {
            try
            {
                string departmenttId = string.Empty;
                if (!string.IsNullOrEmpty(currentUserService.DeptId)) departmenttId = currentUserService.DeptId;
                else departmenttId =Convert.ToString(deptId);

                var param = new DynamicParameters();
                param.Add("p_UserId", UserId, DbType.String);
                var dapper = dapperresolverInstance.Resolve(departmenttId);
                var result = await Task.FromResult(dapper.Get<HostelProfileModel>("usp_GetHostelProfile", param, commandType: CommandType.StoredProcedure));
                return result;
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(Convert.ToString(ex));
                await errorLogger.Log("usp_GetHostelProfile", ex);
                return null;
            }
        }
        public async Task<DepartmentProfileModel?> GetDepartmentDetails(string UserId, int? EntityRole, int? deptId)
        {
            try
            {
                int? departmentId = 0;

                if(deptId>0)
                    departmentId=deptId;
                else
                    departmentId=Convert.ToInt32(currentUserService.DeptId);

                
                var param = new DynamicParameters();
                param.Add("p_UserId", UserId.ToString(), DbType.String);
                param.Add("p_EntityRole", EntityRole, DbType.Int32);
                var dapper = dapperresolverInstance.Resolve(Convert.ToString(departmentId));
                //var result = await Task.FromResult(dapper.Get<DepartmentProfileModel>("usp_GetDepartmentDetails", param, commandType: CommandType.StoredProcedure));
                var result = await Task.FromResult(dapper.Get<DepartmentProfileModel>("usp_GetDepartmentDetailsNew", param, commandType: CommandType.StoredProcedure));
                return result;
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(Convert.ToString(ex));
                //await errorLogger.Log("usp_GetDepartmentDetails", ex);
                await errorLogger.Log("usp_GetDepartmentDetailsNew", ex);
                return null;
            }
        }
        public async Task<bool?> UpdateFirstLogin(string Id)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("p_UserId", Id, DbType.String);
                var dapper = dapperresolverInstance.Resolve(currentUserService.DeptId);
                var result = await Task.FromResult(dapper.Execute("usp_UpdateScrutinyUserFirstLogin", param, commandType: CommandType.StoredProcedure));                
                return true;
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(Convert.ToString(ex));
                await errorLogger.Log("usp_UpdateScrutinyUserFirstLogin", ex);
                return false;
            }
        }
        public async Task<bool?> UpdateFirstLoginToTrue(string userId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("p_userId", userId, DbType.String);
                var dapper = dapperresolverInstance.Resolve(currentUserService.DeptId);
                var result = await Task.FromResult(dapper.Execute("usp_UpdateFirstLoginForResetPassword", param, commandType: CommandType.StoredProcedure));
                return true;
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(Convert.ToString(ex));
                await errorLogger.Log("usp_UpdateFirstLoginForResetPassword", ex);
                return false;
            }
        }
        public async Task<bool> UpdateAadhaar(string UIDReference,string Name,int Gender)
        {
            try
            {
                var param = new DynamicParameters();               
                param.Add("p_UserId", currentUserService.UserId, DbType.String);
                param.Add("p_UIDReference", UIDReference, DbType.Int64);
                param.Add("p_Name", Name, DbType.String);
                param.Add("p_Gender", Gender, DbType.Int32);
                var dapper = dapperresolverInstance.Resolve(currentUserService.DeptId);
                var result = await Task.FromResult(dapper.Execute("usp_UpdateAadhaar", param, commandType: CommandType.StoredProcedure));
                return true;
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(Convert.ToString(ex));
                await errorLogger.Log("usp_UpdateAadhaar", ex);
                return false;
            }
        }
    }
}
