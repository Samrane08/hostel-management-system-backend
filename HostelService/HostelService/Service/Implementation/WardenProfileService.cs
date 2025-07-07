using Dapper;
using Helper;
using Model;
using Repository.Interface;
using Service.Interface;
using System.Data;

namespace Service.Implementation
{
    public class WardenProfileService : IWardenProfileService
    {
        private readonly IDapperResolver dapperresolverInstance;
        private readonly ICurrentUserService currentUserService;
        private readonly IErrorLogger errorLogger;

        public WardenProfileService(IDapperResolver dapperresolverInstance, ICurrentUserService currentUserService, IErrorLogger errorLogger)
        {
            this.dapperresolverInstance = dapperresolverInstance;
            this.currentUserService = currentUserService;
            this.errorLogger = errorLogger;
        }
        public async Task<WardenProfileModel?> GetWardenProfile()
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("p_UserId", currentUserService.UserId, DbType.String);
                var dapper = dapperresolverInstance.Resolve(currentUserService.DeptId);
                var result = await Task.FromResult(dapper.Get<WardenProfileModel>("usp_hostel_GetWardenProfile", param, commandType: CommandType.StoredProcedure));
                return result;
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(Convert.ToString(ex));
                await errorLogger.Log("usp_hostel_GetWardenProfile", ex);
                return null;
            }
        }
        public async Task<WardenProfileModel?> SaveWardenProfile(WardenProfileModel model)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("p_UserId", currentUserService.UserId, DbType.String);
                param.Add("p_Name", model.Name, DbType.String);
                param.Add("p_Gender", model.Gender, DbType.Int32);
                param.Add("p_Designation", model.Designation, DbType.String);
                param.Add("p_Mobile", model.Mobile, DbType.String);
                param.Add("p_Email", model.Email, DbType.String);
                param.Add("p_DOB", model.DOB, DbType.String);
                param.Add("p_WardenCharge", model.WardenCharge, DbType.Int32);
                param.Add("p_EmergencyContact", model.Mobile, DbType.String);
                var dapper = dapperresolverInstance.Resolve(currentUserService.DeptId);
                var result = await Task.FromResult(dapper.Insert<WardenProfileModel>("usp_hostel_UpdateWardenProfile", param, commandType: CommandType.StoredProcedure));
                return result;
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(Convert.ToString(ex));
                await errorLogger.Log("usp_hostel_UpdateWardenProfile", ex);
                return null;
            }
        }
        public async Task<List<ResponseNoticeModel?>> GetNotices()
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("_UserId", currentUserService.HostelId, DbType.Int32);
                var dapper = dapperresolverInstance.Resolve(currentUserService.DeptId);
                var result = await Task.FromResult(dapper.GetAll<ResponseNoticeModel>("usp_GetWardenNoticesByUserId", param, commandType: CommandType.StoredProcedure));
                return result;
            }
            catch (Exception ex)
            {
                await errorLogger.Log("usp_GetWardenNoticesByUserId", ex);
                return new List<ResponseNoticeModel?>();
            }
        }
        public async Task<ResponseNoticeModel> SaveNotices(ReqNoticeModel model)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("_UserId", currentUserService.UserId, DbType.String);
                param.Add("_HostelId", currentUserService.HostelId, DbType.Int32);
                param.Add("_NotificationText", model.NotificationText, DbType.String);
                param.Add("_NotificationHeader", model.NotificationHeader, DbType.String);
                param.Add("_DocumentId", model.DocumentId, DbType.String);
                var dapper = dapperresolverInstance.Resolve(currentUserService.DeptId);
                var result = await Task.FromResult(dapper.Insert<ResponseNoticeModel>("usp_InsertWardenNotice", param, commandType: CommandType.StoredProcedure));
                return result;
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(Convert.ToString(ex));
                await errorLogger.Log("usp_InsertWardenNotice", ex);
                return new ResponseNoticeModel();
            }
        }
        public async Task<bool> DeleteNitice(long Id)
        {
            try
            {
                var myParams = new DynamicParameters();
                myParams.Add("_UserId", currentUserService.HostelId, DbType.Int32);
                myParams.Add("_ID", Id, DbType.Int64);
                myParams.Add("Result", dbType: DbType.Byte, direction: ParameterDirection.Output);
                var dapper = dapperresolverInstance.Resolve(currentUserService.DeptId);
                await Task.FromResult(dapper.Execute("usp_deleteWardenNotices", myParams, CommandType.StoredProcedure));
                bool result = myParams.Get<byte>("Result") == 1;
                return result;
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(Convert.ToString(ex));
                await errorLogger.Log("usp_deleteWardenNotices", ex);
                return false;
            }
        }
        public async Task WardenMap(WardenMapModel model)
        {
            var query = $"INSERT INTO `hostelmanagement`.`hostel_wardener_mapping` (`HostelId`,`UserIdentity`,`Name`,`Mobile`,`IsFirstLogin`,`CreatedBy`,`CreatedOn`) VALUES ({model.HostelId},'{model.UserIdentity}','{model.Name}','{model.Mobile}',1,'admin',now());";
            var dapper = dapperresolverInstance.Resolve(currentUserService.DeptId);
            await Task.FromResult(dapper.Execute(query, null, commandType: CommandType.Text));
        }
    }
}
