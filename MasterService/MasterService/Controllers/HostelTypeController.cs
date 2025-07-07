using MasterService.Models;
using MasterService.Service.Interface;
using MasterService.Service.Utility;
using Microsoft.AspNetCore.Mvc;

namespace MasterService.Controllers
{

    public class HostelTypeController : APIBaseController
    {
        private readonly IDapper dapper;
        private readonly IErrorLogger errorLogger;
        public HostelTypeController(IDapper dapper, IErrorLogger errorLogger)
        {
            this.dapper = dapper;
            this.errorLogger = errorLogger;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var result = await Task.FromResult(dapper.GetAll<SelectListModel>("usp_GetHostelType", null, commandType: System.Data.CommandType.StoredProcedure));
                return Ok(result);
            }
            catch (Exception ex)
            {
                // ExceptionLogging.LogException(Convert.ToString(ex));
                await errorLogger.Log("usp_GetHostelType", ex);
                return Ok(new List<string>());
            }
        }
    }
}
