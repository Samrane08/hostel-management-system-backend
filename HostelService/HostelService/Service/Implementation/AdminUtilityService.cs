using Dapper;
using Helper;
using Model;
using Repository.Interface;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Implementation
{
    public class AdminUtilityService : IAdminUtilityService
    {
        private readonly IDapperResolver dapperresolverInstance;
        private readonly IErrorLogger errorLogger;

        private readonly ICurrentUserService currentUserService;
        public AdminUtilityService(IDapperResolver dapperresolverInstance, IErrorLogger errorLogger, ICurrentUserService currentUserService)
        {
            this.dapperresolverInstance = dapperresolverInstance;
            this.errorLogger = errorLogger;
            this.currentUserService = currentUserService;
        }
        public async Task<string> GetPaymentStatusAsync(string applicationNo)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("p_ApplicationNo", applicationNo, DbType.String);
                var dapper = dapperresolverInstance.Resolve("1");
                var result = await Task.FromResult(dapper.Get<string>("usp_GetPaymentStatus", param, commandType: CommandType.StoredProcedure));
                return result;
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(Convert.ToString(ex));
                return null;
            }
        }
        public async Task<bool> VerifySuperAdminMobile(string MobileNo)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("p_MobileNo", MobileNo, DbType.String);
                var dapper = dapperresolverInstance.Resolve("1");
                var result = await Task.FromResult(dapper.Get<int>("usp_VerifySuperAdminMobile", param, commandType: CommandType.StoredProcedure));
                if (result == 1)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(Convert.ToString(ex));
                return false;
            }
        }
        public async Task<string> GetApplicationIdAsync(string applicationNo)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("p_ApplicationNo", applicationNo, DbType.String);
                var dapper = dapperresolverInstance.Resolve("1");
                var result = await Task.FromResult(dapper.Get<string>("usp_GetApplicationId", param, commandType: CommandType.StoredProcedure));
                return result;
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(Convert.ToString(ex));
                return null;
            }
        }

        public async Task<UserAadhaarModel> GetApplicationByAadharRef(string aadharRefNo)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("p_UID", aadharRefNo, DbType.String);
                var dapper = dapperresolverInstance.Resolve("1");
                var result = await Task.FromResult(dapper.Get<UserAadhaarModel>("usp_GetDetailByAAdhar", param, commandType: CommandType.StoredProcedure));
                return result;
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(Convert.ToString(ex));
                throw ex;
            }
        }
        public async Task<UserAadhaarModel> GetApplicationByEmail(string email)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("p_Email", email, DbType.String);
                var dapper = dapperresolverInstance.Resolve("1");
                var result = await Task.FromResult(dapper.Get<UserAadhaarModel>("usp_GetDetailByEmail", param, commandType: CommandType.StoredProcedure));
                return result;
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(Convert.ToString(ex));
                throw ex;
            }
        }
        public async Task<UserAadhaarModel> GetApplicationByMobile(string mobile)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("p_Mobile", mobile, DbType.String);
                var dapper = dapperresolverInstance.Resolve("1");
                var result = await Task.FromResult(dapper.Get<UserAadhaarModel>("usp_GetDetailByMobile", param, commandType: CommandType.StoredProcedure));
                return result;
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(Convert.ToString(ex));
                throw ex;
            }
        }

        public async Task<string> GetAyApplicationDetails()//  check this 
        {
            try
            {
                var param = new DynamicParameters();
                var dapper = dapperresolverInstance.Resolve(currentUserService.DeptId);
                var result = await Task.FromResult(dapper.Get<string>("Usp_GetAyApplicationDetails", null, commandType: CommandType.StoredProcedure));
                return result;
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(Convert.ToString(ex));
                await errorLogger.Log("Usp_GetAyApplicationDetails", ex);
                return "";
            }
        }


        public async Task<int> updateClosingDate(ClosingDateModel model)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("p_id", model.id, DbType.String);
                // param.Add("p_closing_date", model.closingDate, DbType.String);
                DateTime parsedDate;
                if (DateTime.TryParseExact(model.closingDate, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedDate))
                {
                    param.Add("p_closing_date", model.closingDate, DbType.String);
                }
                else
                {
                    throw new Exception("Invalid date format received.");
                }
                //int result = await Task.FromResult(dapper.Execute("usp_UpdateApplicationClosingDate", param, CommandType.StoredProcedure));
                var dapper = dapperresolverInstance.Resolve(model.DeptId);
                //var dapper = dapperresolverInstance.Resolve("1");
                var result = await Task.FromResult(dapper.Insert<int>("usp_UpdateApplicationClosingDate", param, commandType: CommandType.StoredProcedure));
                return result;
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(Convert.ToString(ex));
                throw ex;
            }
        }

        public async Task<List<ClosingDateModel1>> GetUpdateClosingDate(string DeptId)
        {
            try
            {
                //var dapper = dapperresolverInstance.Resolve("1");
                //var deptid = ToString(DeptId)
                var deptid = DeptId?.ToString();
                var dapper = dapperresolverInstance.Resolve(deptid);
                var result = await Task.FromResult(dapper.GetAll<ClosingDateModel1>("usp_GetApplicationClosingDate", null, commandType: CommandType.StoredProcedure));
                return result;

            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(Convert.ToString(ex));
                throw ex;
            }
        }




        public async Task<List<ServiceTypeClosingDateModel>> GetServiceTypeClosingDate()
        {
            try
            {
                var dapper = dapperresolverInstance.Resolve("1");
          
                var result = await Task.FromResult(dapper.GetAll<ServiceTypeClosingDateModel>("usp_ServicesExpireDropDown", null, commandType: CommandType.StoredProcedure));
                return result;
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(Convert.ToString(ex));
                throw ex;
            }
        }

        public async Task<List<ParentIdMenuMapping>> GetparentIdAddMenuMapping()
        {
            try
            {
                var dapper = dapperresolverInstance.Resolve("1");
                //var dapper = dapperresolverInstance.Resolve(deptid);
                var result = await Task.FromResult(dapper.GetAll<ParentIdMenuMapping>("usp_ParentIdMenuAddDetails", null, commandType: CommandType.StoredProcedure));
                return result;
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(Convert.ToString(ex));
                throw ex;
            }
        }

        public async Task<string> SaveMenubtnMapping(MenuInsertModel model)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("p_MenuName", model.menuName, DbType.String);
                param.Add("p_MenuNameMr", model.menuNameMr, DbType.String);
                param.Add("p_ParentId", model.parentId, DbType.Int16);
                param.Add("p_Url", model.url, DbType.String);
                param.Add("p_Icon", model.icon, DbType.String);
                param.Add("p_Status", model.status, DbType.Int16);
                param.Add("p_Sort", model.sort, DbType.Int16);

                var dapper = dapperresolverInstance.Resolve("1");
                var result = await Task.FromResult(dapper.Insert<string>("Usp_UpsertMenuAddDetails", param, commandType: CommandType.StoredProcedure));
                return result;
            }
            catch (Exception ex)
            {
                // ExceptionLogging.LogException(Convert.ToString(ex));
                throw ex;
            }
        }


        public async Task<List<RoleAccordDept>> GetRoleListDeptid(string departmentid)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("p_Department", departmentid, DbType.String);
                //var dapper = dapperresolverInstance.Resolve(currentUserService.DeptId);
                var dapper = dapperresolverInstance.Resolve("1");
                var result = await Task.FromResult(dapper.GetAll<RoleAccordDept>("usp_GetRoles", param, commandType: CommandType.StoredProcedure));
                return result;
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(Convert.ToString(ex));
                throw ex;
            }
        }

        public async Task<List<MenuMapping>> GetMenuListAll()
        {
            try
            {
                var dapper = dapperresolverInstance.Resolve("1");
                var result = await Task.FromResult(dapper.GetAll<MenuMapping>("usp_GetAllMenuList", null, commandType: CommandType.StoredProcedure));

                return result;
            }
            catch (Exception ex)
            {
                // ExceptionLogging.LogException(Convert.ToString(ex));
                throw;
            }
        }


        public async Task<string> SaveMenuMapping(MenuMappingInsert model)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("p_roleId", model.EntityMappingId, DbType.String);
                param.Add("p_MenuId", model.MenuId, DbType.Int16);
                param.Add("p_Status", model.Status, DbType.Int16);
                var dapper = dapperresolverInstance.Resolve("1");
                var result = await Task.FromResult(dapper.Insert<string>("Usp_UpsertMenuMapping", param, commandType: CommandType.StoredProcedure));
                return result;
            }
            catch (Exception ex)
            {
                // ExceptionLogging.LogException(Convert.ToString(ex));
                throw ex;
            }
        }


    }
}
