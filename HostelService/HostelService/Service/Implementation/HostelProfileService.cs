using Dapper;
using Helper;
using Model;
using Repository.Interface;
using Service.Interface;
using System.Data;

namespace Service.Implementation
{
    public class HostelProfileService: IHostelProfileService
    {
        private readonly IDapperResolver dapperresolverInstance;
        private readonly ICurrentUserService currentUserService;
        private readonly IErrorLogger errorLogger;

        public HostelProfileService(IDapperResolver dapperresolverInstance, ICurrentUserService currentUserService,IErrorLogger errorLogger)
        {
            this.dapperresolverInstance = dapperresolverInstance;
            this.currentUserService = currentUserService;
            this.errorLogger = errorLogger;
        }
        public async Task<HostelProfileModel?> GetHostelProfile()
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("p_UserId", currentUserService.UserId, DbType.String);
                var dapper = dapperresolverInstance.Resolve(currentUserService.DeptId);
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
        public async Task<HostelProfileModel?> GetHostelProfileByHostelId(int? hostelId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("p_HostelId", hostelId, DbType.Int32);
                var dapper = dapperresolverInstance.Resolve(currentUserService.DeptId);
                var result = await Task.FromResult(dapper.Get<HostelProfileModel>("usp_GetHostelProfileByHostelId", param, commandType: CommandType.StoredProcedure));
                return result;
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(Convert.ToString(ex));
                await errorLogger.Log("usp_GetHostelProfile", ex);
                return null;
            }
        }
        public async Task<HostelProfileModel?> SaveHostelProfile(HostelProfileModel model)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("p_UserId", currentUserService.UserId, DbType.String);
                param.Add("p_HostelID", model.HostelID, DbType.Int32);
                param.Add("p_Address", model.Address, DbType.String);
                param.Add("p_Mobile", model.Mobile, DbType.String);
                param.Add("p_Landline", model.Landline, DbType.String);
                param.Add("p_Email", model.Email, DbType.String);
                param.Add("p_Capacity", model.Capacity, DbType.Int32);
                param.Add("p_BuildingCapacity", model.BuildingCapacity, DbType.Int32);
                //param.Add("p_HostelType", model.HostelType, DbType.Int32); 
                var dapper = dapperresolverInstance.Resolve(currentUserService.DeptId);
                var result = await Task.FromResult(dapper.Insert<HostelProfileModel>("usp_HostelProfileUpdate", param, commandType: CommandType.StoredProcedure));
                return result;
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(Convert.ToString(ex));
                await errorLogger.Log("usp_HostelProfileUpdate", ex);
                return null;
            }
        }
    }
}
