using Dapper;
using Helper;
using Model;
using Model.LotteryModel;
using Newtonsoft.Json;
using Repository.Interface;
using Service.Interface;
using System.Data;

namespace Service.Implementation
{
    public class DirectWardenService : IDirectWardenService
    {
        private readonly IDapperResolver dapperresolverInstance;
        private readonly ICurrentUserService currentUserService;
        private readonly IErrorLogger errorLogger;

        public DirectWardenService(IDapperResolver dapperresolverInstance, ICurrentUserService currentUserService,IErrorLogger errorLogger)
        {
            this.dapperresolverInstance = dapperresolverInstance;
            this.currentUserService = currentUserService;
            this.errorLogger = errorLogger;
        }

        
        public async Task<List<DirectWardenVacancyModel>> GetDirectWardenData(int? HostelID)
        {
            try
            {
           
                var param = new DynamicParameters();
                param.Add("p_HostelID", string.IsNullOrEmpty(Convert.ToString(HostelID)) ? 0 : HostelID, DbType.Int32);
                //param.Add("p_workflowId", currentUserService.WorkFlowId, DbType.Int32);
                param.Add("p_workflowId", !string.IsNullOrWhiteSpace(currentUserService.UserId) && string.IsNullOrEmpty(currentUserService.WorkFlowId) ? 1 : currentUserService.WorkFlowId, DbType.Int32);
                param.Add("p_UserId", currentUserService.UserId, DbType.String);
                var dapper = dapperresolverInstance.Resolve(currentUserService.DeptId);
                var result = await Task.FromResult(dapper.GetAll<DirectWardenVacancyModel>("usp_GetDataFromDirectWardenVacancy", param, commandType: CommandType.StoredProcedure));
                return result;
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(Convert.ToString(ex));
                await errorLogger.Log("usp_GetDataFromDirectWardenVacancy", ex);
                return null;
            }
        }

        public async Task<List<DropDownData>> GetPriorityQuotaData()
        {
            try
            {
                var dapper = dapperresolverInstance.Resolve(currentUserService.DeptId);
                var result = await Task.FromResult(dapper.GetAll<DropDownData>("usp_GetPriorityData", null, commandType: CommandType.StoredProcedure));
                return result;
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(Convert.ToString(ex));
                await errorLogger.Log("usp_GetPriorityData", ex);
                return null;
            }
        }

        public async Task<int> PostDirectWardenData(List<DirectWardenVacancyModel> models,int? HostelId)
        {
            try
            {
                var param = new DynamicParameters();
                string jsonData = JsonConvert.SerializeObject(models);
                param.Add("p_UserId", currentUserService.UserId, DbType.String);

                if (!string.IsNullOrWhiteSpace(currentUserService.UserId) && string.IsNullOrEmpty(currentUserService.WorkFlowId))
                {
                    param.Add("p_HostelId", string.IsNullOrEmpty(currentUserService.HostelId) ? 0 : currentUserService.HostelId, DbType.Int32);
                    param.Add("p_workflowId", 1, DbType.Int32);
                }
                
                if (currentUserService.WorkFlowId == "3")
                {
                    param.Add("p_HostelId", HostelId, DbType.Int32);
                    param.Add("p_workflowId", currentUserService.WorkFlowId, DbType.Int32);
                }
                param.Add("jsonData", jsonData, DbType.String);
                var dapper = dapperresolverInstance.Resolve(currentUserService.DeptId);
                var result = await Task.FromResult(dapper.Insert<int>("InsertBulkDataIntoDirectWardenVacancy", param, commandType: CommandType.StoredProcedure));
                return result;
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(Convert.ToString(ex));
                await errorLogger.Log("InsertBulkDataIntoDirectWardenVacancy", ex);
                return 0;
            }
        }

     
    }
}
