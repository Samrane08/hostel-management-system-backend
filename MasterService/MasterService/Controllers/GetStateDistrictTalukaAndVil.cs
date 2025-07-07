using Dapper;
using MasterService.Models;
using MasterService.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace MasterService.Controllers
{
    public class GetStateDistrictTalukaAndVil : APIBaseController
    {

        private readonly IDapper dapper;
        private readonly IErrorLogger errorLogger;

        public GetStateDistrictTalukaAndVil(IDapper dapper, IErrorLogger errorLogger)
        {
            this.dapper = dapper;
            this.errorLogger = errorLogger;
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Get(string dynamicparams, int dynamicId)
        {
            try
            {
                var myparam = new DynamicParameters();
                myparam.Add("p_dynamicparam", dynamicparams, DbType.String);
                myparam.Add("p_dynamicId", dynamicId, DbType.Int32);

                var result = await Task.FromResult(dapper.GetAll<SelectListModel>("usp_GetStateDistrictTalukaAndVil", myparam, commandType: System.Data.CommandType.StoredProcedure));
                return Ok(result);
            }
            catch (Exception ex)
            {
                // ExceptionLogging.LogException(Convert.ToString(ex));
                await errorLogger.Log("usp_GetStateDistrictTalukaAndVil", ex);
                return Ok(new List<string>());
            }
        }
    }
}
