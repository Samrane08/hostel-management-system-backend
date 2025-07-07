using Dapper;
using MasterService.Models;
using MasterService.Service.Interface;
using MasterService.Service.Utility;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Data;

namespace MasterService.Controllers
{

    public class IssuingAuthorityController : APIBaseController
    {
        private readonly IDapper dapper;
        private readonly IErrorLogger errorLogger;

        public IssuingAuthorityController(IDapper dapper, IErrorLogger errorLogger)
        {
            this.dapper = dapper;
            this.errorLogger = errorLogger;
        }

        [HttpGet]
        public async Task<IActionResult> get(string drpType)
        {
            try
            {
                var myparam = new DynamicParameters();
                myparam.Add("p_DrpType", drpType, DbType.String);
                var result = await Task.FromResult(dapper.GetAll<SelectListModel>("usp_GetIssuingAuthority", myparam, commandType: System.Data.CommandType.StoredProcedure));
                return Ok(result);
            }
            catch (Exception ex)
            {
                // ExceptionLogging.LogException(Convert.ToString(ex));
                await errorLogger.Log("usp_GetIssuingAuthority", ex);
                return Ok(new List<string>());
            }
        }
    }
}
