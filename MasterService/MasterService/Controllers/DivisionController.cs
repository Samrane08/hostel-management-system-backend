using MasterService.Models;
using MasterService.Models.ReqModel;
using MasterService.Service.Interface;
using MasterService.Service.Utility;
using Microsoft.AspNetCore.Mvc;

namespace MasterService.Controllers
{
    public class DivisionController : APIBaseController
    {
        private readonly IDapper dapper;
        private readonly ICacheService cacheService;
        private readonly IErrorLogger errorLogger;

        public DivisionController(IDapper dapper, ICacheService cacheService, IErrorLogger errorLogger)
        {
            this.dapper = dapper;
            this.cacheService = cacheService;
            this.errorLogger = errorLogger;
        }
        [HttpGet]
        public async Task<IActionResult> Get(int? lang = 1)
        {
            try
            {
                var response = new List<SelectListModel>();
                var result = await cacheService.GetOrCreateAsync("DivisionList", async () => { return await DivisionList(); }, TimeSpan.FromDays(30));
               
                if (lang == 2)
                    response = result.Select(x => new SelectListModel { Value = x.DivisionCode.ToString(), Text = x.DivisionMr }).ToList();
                else
                    response = result.Select(x => new SelectListModel { Value = x.DivisionCode.ToString(), Text = x.DivisionName }).ToList();

                return Ok(response);
            }
            catch (Exception ex)
            {
                // ExceptionLogging.LogException(Convert.ToString(ex));
                return Ok(new List<string>());
            }
        }
        private async Task<List<DivisionModel>> DivisionList()
        {
            try
            {   //usp_GetTalukaList
                var result = await Task.FromResult(dapper.GetAll<DivisionModel>("usp_DivisionMaster", null, commandType: System.Data.CommandType.StoredProcedure));
                return result;
            }
            catch (Exception ex)
            {
                // ExceptionLogging.LogException(Convert.ToString(ex));
                await errorLogger.Log("usp_DivisionMaster", ex);

                return new List<DivisionModel>();
            }
        }
    }
}
