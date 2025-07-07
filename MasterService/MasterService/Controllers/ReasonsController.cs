using MasterService.Models;
using MasterService.Service.Interface;
using MasterService.Service.Utility;
using Microsoft.AspNetCore.Mvc;

namespace MasterService.Controllers
{
    public class ReasonsController : APIBaseController
    {
        private readonly IDapper dapper;
        public ReasonsController(IDapper dapper)
        {
            this.dapper = dapper;
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var result = await Task.FromResult(dapper.GetAll<SelectListModel>("usp_GetRejectionReasons", null, commandType: System.Data.CommandType.StoredProcedure));
                return Ok(result);
            }
            catch (Exception ex)
            {
                // ExceptionLogging.LogException(Convert.ToString(ex));
                return Ok(new List<string>());
            }
        }

        [HttpGet("offline-scrutiny-reasons")]
        public async Task<IActionResult> GetOfflineScrutinyReasons()
        {
            try
            {
                var result = await Task.FromResult(dapper.GetAll<SelectListModel>("usp_GetOfflineScrutinyRejectReasons", null, commandType: System.Data.CommandType.StoredProcedure));
                return Ok(result);
            }
            catch (Exception ex)
            {
                // ExceptionLogging.LogException(Convert.ToString(ex));
                return Ok(new List<string>());
            }
        }
    }
}
