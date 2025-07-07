using MasterService.Models;
using MasterService.Service.Interface;
using MasterService.Service.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MasterService.Controllers
{

    public class GenderController : APIBaseController
    {
        private readonly IDapper dapper;
        private readonly IErrorLogger errorLogger;

        public GenderController(IDapper dapper, IErrorLogger errorLogger)
        {
            this.dapper = dapper;
            this.errorLogger = errorLogger;
        }
        [HttpGet]
        public async Task<IActionResult> get()
        {
            try
            {
                var result = await Task.FromResult(dapper.GetAll<SelectListModel>("usp_GetGenderList", null, commandType: System.Data.CommandType.StoredProcedure));
                return Ok(result);
            }
            catch (Exception ex)
            {
                // ExceptionLogging.LogException(Convert.ToString(ex));
                await errorLogger.Log("usp_GetGenderList", ex);
                return Ok(new List<string>());
            }
        }


        [HttpGet("get-premises")]
        [AllowAnonymous]
        public async Task<IActionResult> GetPremises()
        {
            try
            {
                var result = await Task.FromResult(dapper.GetAll<SelectListModel>("usp_GetPremisesList", null, commandType: System.Data.CommandType.StoredProcedure));
                return Ok(result);
            }
            catch (Exception ex)
            {
                //ExceptionLogging.LogException(Convert.ToString(ex));
                await errorLogger.Log("usp_GetPremisesList", ex);
                return Ok(new List<string>());
            }
        }




    }
}
