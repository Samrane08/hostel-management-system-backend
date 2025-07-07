using MasterService.Models.ReqModel;
using MasterService.Service.Interface;
using MasterService.Service.Utility;
using Microsoft.AspNetCore.Mvc;

namespace MasterService.Controllers
{
    public class SwadharServiceTypeController : APIBaseController
    {
        private readonly IDapper dapper;
        private readonly ICacheService cacheService;

        public SwadharServiceTypeController(IDapper dapper, ICacheService cacheService)
        {
            this.dapper = dapper;
            this.cacheService = cacheService;
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var result = await cacheService.GetOrCreateAsync("SwadharServiceList", async () => { return await SwadharServiceList(); }, TimeSpan.FromDays(30));
                if(result == null || result.Count == 0)
                {
                    return Ok(await SwadharServiceList());
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                // ExceptionLogging.LogException(Convert.ToString(ex));
                return Ok(new List<string>());
            }
        }
        private async Task<List<SwadharServiceType>> SwadharServiceList()
        {
            try
            {  
                var result = await Task.FromResult(dapper.GetAll<SwadharServiceType>("usp_SwadharServiceMaster", null, commandType: System.Data.CommandType.StoredProcedure));
                return result;
            }
            catch (Exception ex)
            {
                // ExceptionLogging.LogException(Convert.ToString(ex));
                return new List<SwadharServiceType>();
            }
        }
    }
}
