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
    public class AdmittedRegistrationService:IAdmittedRegistrationService
    {
        private readonly IDapperResolver dapperresolverInstance;
        private readonly ICurrentUserService currentUserService;
        private readonly IErrorLogger errorLogger;

        public AdmittedRegistrationService(IDapperResolver dapperresolverInstance, ICurrentUserService currentUserService, IErrorLogger errorLogger)
        {
            this.dapperresolverInstance = dapperresolverInstance;
            this.currentUserService = currentUserService;
            this.errorLogger = errorLogger;
        }
        public bool IsRecordValidate(AdmittedAadhharList model)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(model?.UIDNo))
                {
                    return false;
                }
                if (model?.UIDNo?.Length != 12)
                {
                    return false;
                }
                if (int.Parse(model?.UIDNo.Substring(0, 1)) < 2)
                {
                    return false;
                }
                if (!AadhaarVerifyAlgorithm.ValidateAadhaar(model?.UIDNo))
                {
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(Convert.ToString(ex));
                return false;
            }
        }
        public async Task Registration(string JsonData, string CourseType)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("p_HostelId", currentUserService.HostelId, DbType.Int32);
                param.Add("p_UserId", currentUserService.UserId, DbType.String);
                param.Add("p_CourseType", CourseType, DbType.Int32);
                param.Add("@jsonArray", JsonData, DbType.String);
                var dapper = dapperresolverInstance.Resolve(currentUserService.DeptId);
                await Task.FromResult(dapper.Insert<int>("usp_UpdateAdmittedApplicant", param, commandType: CommandType.StoredProcedure));
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(Convert.ToString(ex));
                await errorLogger.Log("usp_UpdateAdmittedApplicant", ex);
            }
        }
    }
}
