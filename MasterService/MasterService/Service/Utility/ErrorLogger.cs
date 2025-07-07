using Dapper;
using MasterService.Service.Interface;
using System.Data;
using Microsoft.Extensions.DependencyInjection;


namespace MasterService.Service.Utility
{
    

        public class ErrorLogger : IErrorLogger
        {
            private readonly IDapper dapper;
            private readonly ICurrentUserService currentUserService;
            public ErrorLogger(IDapper dapper, ICurrentUserService currentUserService)
            {
                this.dapper = dapper;
                this.currentUserService = currentUserService;
            }
            public async Task Log(string ErrorAt, Exception ex)
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
                await Task.FromResult(dapper.Execute("usp_MasterErrorLogger", param, commandType: CommandType.StoredProcedure));
            }
            public async Task CustomLog(string ErrorAt, string cust)
            {
                string error = string.Empty;

                var param = new DynamicParameters();
                param.Add("error_at", ErrorAt, DbType.String);
                param.Add("exception", cust, DbType.String);
                param.Add("createdby", "12", DbType.String);
                await Task.FromResult(dapper.Execute("usp_MasterErrorLogger", param, commandType: CommandType.StoredProcedure));
            }
        }
}

