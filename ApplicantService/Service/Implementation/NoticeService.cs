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
    public class NoticeService : INoticeService
    {
        private readonly IDapper dapper;
        private readonly ICurrentUserService currentUserService;
        private readonly IErrorLogger errorLogger;
        public NoticeService(IDapper dapper, ICurrentUserService currentUserService, IErrorLogger errorLogger)
        {
            this.dapper = dapper;
            this.currentUserService = currentUserService;
            this.errorLogger = errorLogger;
        }
        public async Task<List<FetchHostelNoticesModel?>> GetNotices()
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("p_UserId", currentUserService.UserNumericId, DbType.Int64);
                var result = await Task.FromResult(dapper.GetAll<FetchHostelNoticesModel>("usp_GetNoticesApplicant", param, commandType: CommandType.StoredProcedure));
                return result;
            }
            catch (Exception ex)
            {
                // ExceptionLogging.LogException(Convert.ToString(ex));
                await errorLogger.Log("usp_GetNoticesApplicant", ex);
                return null;
            }
        }
    }
}
