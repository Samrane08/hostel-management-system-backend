using Dapper;
using Helper;
using Model;
using Repository.Interface;
using Service.Interface;
using System.Data;

namespace Service.Implementation;
public class ScrutinyService : IScrutinyService
{
    private readonly IDapperResolver dapperresolverInstance;
    private readonly ICurrentUserService currentUserService;
    private readonly IErrorLogger errorLogger;

    public ScrutinyService(IDapperResolver dapperresolverInstance, ICurrentUserService currentUserService, IErrorLogger errorLogger)
    {
        this.dapperresolverInstance = dapperresolverInstance;
        this.currentUserService = currentUserService;
        this.errorLogger = errorLogger;
    }
    public async Task<SrutinyResultModel> Scrutiny(ScrutinyModel model)
    {
        try
        {
            var param = new DynamicParameters();
            param.Add("p_UserId", currentUserService.UserId, DbType.String);
            param.Add("p_EntityRole", currentUserService.RoleEntityId, DbType.Int32);
            param.Add("p_HostelId", !string.IsNullOrWhiteSpace(currentUserService.HostelId) ? currentUserService.HostelId : null, DbType.Int32);
            param.Add("p_ApplicationId", model.ApplicationId, DbType.Int64);
            param.Add("p_ActionID", model.ActionId, DbType.Int32);
            param.Add("p_Remark", model.Remark, DbType.String);
            var dapper = dapperresolverInstance.Resolve(currentUserService.DeptId);
            var result = await Task.FromResult(dapper.Update<SrutinyResultModel>("usp_ScrutinyAction", param, commandType: CommandType.StoredProcedure));
            if (model.RejectionId != null && model.RejectionId.Count > 0)
            {
                foreach (var RejectId in model.RejectionId)
                {
                    await ScrutinyRejectReasonAdd(result.Id, RejectId);
                }
            }
            return result;
        }
        catch (Exception ex)
        {
            ExceptionLogging.LogException(Convert.ToString(ex));
            await errorLogger.Log("usp_ScrutinyAction", ex);
            return new SrutinyResultModel() {Id = 0 ,Message = "Scrutiny action failed" };
        }
    }
    public async Task<List<ActionModel>> Workflow(long ApplicationId)
    {
        try
        {
            var param = new DynamicParameters();
            param.Add("p_RoleEntityId", currentUserService.RoleEntityId, DbType.Int32);
            param.Add("p_ApplicationId", ApplicationId, DbType.Int64);
            param.Add("p_HostelId", !string.IsNullOrWhiteSpace(currentUserService.HostelId) ? currentUserService.HostelId: null , DbType.Int32);
            var dapper = dapperresolverInstance.Resolve(currentUserService.DeptId);
            var result = await Task.FromResult(dapper.GetAll<ActionModel>("usp_GetWorkflowActions", param, commandType: CommandType.StoredProcedure));
            return result;
        }
        catch (Exception ex)
        {
            ExceptionLogging.LogException(Convert.ToString(ex));
            await errorLogger.Log("usp_GetWorkflowActions", ex);
            return new List<ActionModel>();
        }
    }
    public async Task<List<SrutinyRemarkModel>> History(long ApplicationId)
    {
        try
        {
            var param = new DynamicParameters();
            param.Add("p_hostelId", !string.IsNullOrWhiteSpace(currentUserService.HostelId) ? currentUserService.HostelId : null, DbType.Int32);
            param.Add("p_ApplicationId", ApplicationId, DbType.Int64);
            var dapper = dapperresolverInstance.Resolve(currentUserService.DeptId);
            var result = await Task.FromResult(dapper.GetAll<SrutinyRemarkModel>("usp_ScrutinyHistory", param, commandType: CommandType.StoredProcedure));

            if (result != null && result.Count > 0)
            {
                //foreach (var item in result)
                //{
                //    if (item.ScrutinyStatus == 4) // Reject
                //    {
                //        var param2 = new DynamicParameters();
                //        param2.Add("p_ScrutinyDetailsId", item.Id, DbType.Int64);
                //        var result2 = await Task.FromResult(dapper.GetAll<SrutinyRejectModel>("usp_GetScrutinyRejectReasons", param2, commandType: CommandType.StoredProcedure));

                //        if (result2 != null && result2.Count > 0)
                //            item.RejectReason = result2.Select(x => x.Reason).ToList();
                //    }
                //}

                return result;
            }
            else
            {
                return  new List<SrutinyRemarkModel>();
            }
        }
        catch (Exception ex)
        {
            ExceptionLogging.LogException(Convert.ToString(ex));
            await errorLogger.Log("usp_ScrutinyHistory", ex);
            return new List<SrutinyRemarkModel>();
        }
    }
    public async Task ScrutinyRejectReasonAdd(long? ScrutinyDetailsId, int RejectId)
    {
        try
        {
            var param = new DynamicParameters();
            param.Add("p_ScrutinyDetailsId", ScrutinyDetailsId, DbType.Int64);
            param.Add("p_RejectId", RejectId, DbType.Int32);
            param.Add("p_CreatedBy", currentUserService.UserId, DbType.String);
            var dapper = dapperresolverInstance.Resolve(currentUserService.DeptId);
            await Task.FromResult(dapper.Execute("usp_Scrutiny_Reject_Reason", param, commandType: CommandType.StoredProcedure));
        }
        catch (Exception ex)
        {
            ExceptionLogging.LogException(Convert.ToString(ex));
            await errorLogger.Log("usp_Scrutiny_Reject_Reason", ex);
        }
    }
}