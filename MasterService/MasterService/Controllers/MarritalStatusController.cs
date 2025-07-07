using MasterService.Models;
using MasterService.Service.Interface;
using MasterService.Service.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MasterService.Controllers
{
    
    public class MarritalStatusController : APIBaseController
    {
        private readonly IDapper dapper;
        private readonly IErrorLogger errorLogger;
        public MarritalStatusController(IDapper dapper, IErrorLogger errorLogger)
        {
            this.dapper = dapper;
            this.errorLogger = errorLogger;
        }

        [HttpGet]
        public async Task<IActionResult> get()
        {
            try
            {
                var result = await Task.FromResult(dapper.GetAll<SelectListModel>("usp_GetMarritalStatus", null, commandType: System.Data.CommandType.StoredProcedure));
                return Ok(result);
            }
            catch (Exception ex)
            {
                // ExceptionLogging.LogException(Convert.ToString(ex));
                await errorLogger.Log("usp_GetMarritalStatus", ex);
                return Ok(new List<string>());
            }
        }
    }
}
