using Dapper;
using Repository.Interface;
using Service.Interface;
using System.Data;

namespace Service.Implementation
{
    public class ErrorLogger: IErrorLogger
    {
        private readonly IDapperResolver dapperresolverInstance;
        private readonly ICurrentUserService currentUserService;
        public ErrorLogger(IDapperResolver dapperresolverInstance, ICurrentUserService currentUserService)
        {
            this.dapperresolverInstance = dapperresolverInstance;
            this.currentUserService = currentUserService;
        }
        public async Task Log(string ErrorAt,Exception ex)
        {
            string error = string.Empty;
            error += "Message ---\n{0}" + ex.Message;
            error += Environment.NewLine + "Source ---\n{0}" + ex.Source;
            error += Environment.NewLine + "StackTrace ---\n{0}" + ex.StackTrace;
            error += Environment.NewLine + "TargetSite ---\n{0}" + ex.TargetSite;
            if (ex.InnerException != null)
            {
                error += Environment.NewLine + "Inner Exception is {0}" + ex.InnerException;
            }
            if (ex.HelpLink != null)
            {
                error += Environment.NewLine + "HelpLink ---\n{0}" + ex.HelpLink;
            }
            var param = new DynamicParameters();
            param.Add("error_at", ErrorAt, DbType.String);
            param.Add("exception", error, DbType.String);
            param.Add("createdby", currentUserService.UserId, DbType.String);
            var dapper = dapperresolverInstance.Resolve(currentUserService.DeptId);
            await Task.FromResult(dapper.Execute("usp_ErrorLogger", param, commandType: CommandType.StoredProcedure));
        }
        public async Task CustomLog(string ErrorAt, string cust)
        {
            string error = string.Empty;
           
            var param = new DynamicParameters();
            param.Add("error_at", ErrorAt, DbType.String);
            param.Add("exception", cust, DbType.String);
            param.Add("createdby", currentUserService.UserId, DbType.String);
            var dapper = dapperresolverInstance.Resolve(currentUserService.DeptId);
            await Task.FromResult(dapper.Execute("usp_ErrorLogger", param, commandType: CommandType.StoredProcedure));
        }
    }
}
