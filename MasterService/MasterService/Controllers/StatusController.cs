using Dapper;
using MasterService.Models;
using MasterService.Service.Implemetation;
using MasterService.Service.Interface;
using MasterService.Service.Utility;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace MasterService.Controllers
{
    public class StatusController : APIBaseController
    {
        private readonly IDapper dapper;
        private readonly ICurrentUserService currentUserService;

        public StatusController(IDapper dapper, ICurrentUserService currentUserService)
        {
            this.dapper = dapper;
            this.currentUserService = currentUserService;
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {              
                var result = await Task.FromResult(dapper.GetAll<SelectListModel>("usp_GetStatus", null, commandType: System.Data.CommandType.StoredProcedure));
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
