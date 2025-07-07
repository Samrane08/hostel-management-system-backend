using MasterService.Models;
using MasterService.Models.ReqModel;
using MasterService.Service.Interface;
using MasterService.Service.Utility;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace MasterService.Controllers;
public class MultipleVillageController : APIBaseController
{
    private readonly IDapper dapper;
    private readonly ICacheService cacheService;
    private readonly IErrorLogger errorLogger;

    public MultipleVillageController(IDapper dapper, ICacheService cacheService)
    {
        this.dapper = dapper;
        this.cacheService = cacheService;
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] List<int>? talukaId)
    {
        try
        {
            var response = new List<SelectListModel>();
            var result = await cacheService.GetOrCreateAsync("VillageList", async () => { return await VillageList(); }, TimeSpan.FromDays(30));

            if (result != null && result.Count > 0 && talukaId!= null && talukaId.Count > 0)
                result = result.Where(x => x.Lang == 1 && talukaId.Contains(x.TalukaId)).ToList();           

            if (result != null && result.Count > 0)
                response = result.Select(x => new SelectListModel { Value = x.Id.ToString(), Text = x.Name })
                                 .ToList();

            if (response.Count == 0)
            {
                var Data = await VillageList();

                if (Data != null && Data.Count > 0 && talukaId != null && talukaId.Count > 0)
                    result = Data.Where(x => x.Lang == 1 && talukaId.Contains(x.TalukaId)).ToList();

                if (Data != null && Data.Count > 0)
                    response = Data.Select(x => new SelectListModel { Value = x.Id.ToString(), Text = x.Name })
                                     .ToList();
            }
            return Ok(response);
        }
        catch (Exception ex)
        {
            // ExceptionLogging.LogException(Convert.ToString(ex));
            return Ok(new List<string>());
        }
    }

    private async Task<List<VillageModel>> VillageList()
    {
        try
        {   //usp_GetTalukaList
            var result = await Task.FromResult(dapper.GetAll<VillageModel>("usp_VillageMaster", null, commandType: System.Data.CommandType.StoredProcedure));
            return result;
        }
        catch (Exception ex)
        {
            // ExceptionLogging.LogException(Convert.ToString(ex));
            await errorLogger.Log("usp_VillageMaster", ex);

            return new List<VillageModel>();
        }
    }
}