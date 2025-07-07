using Dapper;
using Helper;
using Model;
using Repository.Interface;
using Service.Interface;
using System.Data;

namespace Service.Implementation
{
    public class AdmissionService : IAdmissionService
    {
       
        private readonly ICurrentUserService currentUserService;
        private readonly IErrorLogger errorLogger;
        private readonly IDapperResolver dapperresolverInstance;

        public AdmissionService( ICurrentUserService currentUserService, IErrorLogger errorLogger, IDapperResolver dapperresolverInstance)
        {
            
            this.currentUserService = currentUserService;
            this.errorLogger = errorLogger;
            this.dapperresolverInstance = dapperresolverInstance;
        }
        public async Task<bool> UpdateAdmissionStatus(UpdateAdmissionStatusRequestModel model)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("p_UserId", currentUserService.UserId, DbType.String);
                param.Add("p_ApplicationId", model.AppAllotmentId, DbType.Int64);
                param.Add("p_Status", model.AdmissionStatus, DbType.Int32);
                param.Add("p_Remarks", model.Remarks, DbType.String);
                param.Add("Result", dbType: DbType.Byte, direction: ParameterDirection.Output);
                var dapper = dapperresolverInstance.Resolve(currentUserService.DeptId);
                await Task.FromResult(dapper.Execute("usp_UpdateApplicationAdmissionStatus", param, CommandType.StoredProcedure));
                   bool  result = param.Get<byte>("Result") == 1;
                

                if (result && model.AdmissionStatus == 2)
                {
                    if (model.RejectReasonsIds != null && model.RejectReasonsIds.Count > 0)
                    {
                        foreach (var reasonId in model.RejectReasonsIds)
                        {
                            try
                            {
                                var param1 = new DynamicParameters();
                                param1.Add("p_UserId", currentUserService.UserId, DbType.String);
                                param1.Add("p_ApplicationId", model.AppAllotmentId, DbType.Int64);
                                param1.Add("p_RejectId", reasonId, DbType.Int32);
                                await Task.FromResult(dapper.Insert<object>("usp_InsertApplicationAdmissionRejectReason", param1, CommandType.StoredProcedure));
                            }
                            catch (Exception ex)
                            {
                                await errorLogger.Log("usp_InsertApplicationAdmissionRejectReason", ex);
                            }
                        }
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(Convert.ToString(ex));
                await errorLogger.Log("usp_UpdateApplicationAdmissionStatus", ex);
                return false;
            }

        }
    }
}
