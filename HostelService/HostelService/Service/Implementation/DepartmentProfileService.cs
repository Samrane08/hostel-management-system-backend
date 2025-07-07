using Dapper;
using Helper;
using Model;
using Repository.Interface;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Implementation
{
    public class DepartmentProfileService: IDepartmentProfileService
    {

        private readonly IDapperResolver dapperresolverInstance;
        private readonly ICurrentUserService currentUserService;
        private readonly IErrorLogger errorLogger;

        public DepartmentProfileService(IDapperResolver dapperresolverInstance, ICurrentUserService currentUserService, IErrorLogger errorLogger)
        {
            this.dapperresolverInstance = dapperresolverInstance;
            this.currentUserService = currentUserService;
            this.errorLogger = errorLogger;
        }
        public async Task<DeskProfileModel?> GetDepartmentProfile()
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("p_UserId", currentUserService.UserId, DbType.String);
                var dapper = dapperresolverInstance.Resolve(currentUserService.DeptId);
                var result = await Task.FromResult(dapper.Get<DeskProfileModel>("usp_GetDepartmentProfile", param, commandType: CommandType.StoredProcedure));
                return result;
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(Convert.ToString(ex));
                await errorLogger.Log("usp_GetDepartmentProfile", ex);
                return null;
            }
        }
        public async Task<DeskProfileModel?> SaveDepartmentProfile(DeskProfileModel model)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("p_UserId", currentUserService.UserId, DbType.String);             
                param.Add("p_Mobile", model.Mobile, DbType.String);
                param.Add("p_Email", model.Email, DbType.String);               
                param.Add("p_EmergencyContact", model.EmergencyContact, DbType.String);
                var dapper = dapperresolverInstance.Resolve(currentUserService.DeptId);
                var result = await Task.FromResult(dapper.Insert<DeskProfileModel>("usp_UpdateDepartmentProfile", param, commandType: CommandType.StoredProcedure));
                return result;
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(Convert.ToString(ex));
                await errorLogger.Log("usp_UpdateDepartmentProfile", ex);
                return null;
            }
        }
    }
}
