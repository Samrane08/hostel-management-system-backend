using MasterService.Models;
using MasterService.Models.ReqModel;
using MasterService.Service.Interface;
using MasterService.Service.Utility;
using Microsoft.AspNetCore.Mvc;
using MySqlX.XDevAPI.Common;
using System.Data;

namespace MasterService.Controllers;
public class VillegeController : APIBaseController
{
    private readonly IDapper dapper;
    private readonly ICacheService cacheService;

    public VillegeController(IDapper dapper, ICacheService cacheService)
    {
        this.dapper = dapper;
        this.cacheService = cacheService;
    }

    [HttpGet]
    public async Task<IActionResult> Get(int? taluka, int? lang)
    {
        try
        {
            var response = new List<SelectListModel>();
            try
            {
                var result = await cacheService.GetOrCreateAsync("VillageList", async () => { return await VillageList(); }, TimeSpan.FromDays(30));

                if (result != null && result.Count > 0 && taluka.HasValue)
                    result = result.Where(x => x.TalukaId == taluka).ToList();

                if (result != null && result.Count > 0)
                    result = result.Where(x => x.Lang == lang).ToList();

                if (result != null && result.Count > 0)
                    response = result.Select(x => new SelectListModel { Value = x.Id.ToString(), Text = x.Name })
                                     .ToList();
            }
            catch (Exception ex)
            {
                // ExceptionLogging.LogException(Convert.ToString(ex));
            }

            if (response.Count == 0)
            {
                var Data = await VillageList();

                if (Data != null && Data.Count > 0 && taluka.HasValue)
                    Data = Data.Where(x => x.TalukaId == taluka).ToList();

                if (Data != null && Data.Count > 0)
                    Data = Data.Where(x => x.Lang == lang).ToList();

                if (Data != null && Data.Count > 0)
                    response = Data.Select(x => new SelectListModel { Value = x.Id.ToString(), Text = x.Name })
                                     .ToList();
            }
            return Ok(response);
        }
        catch (Exception ex)
        {
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
            return new List<VillageModel>();
        }
    }
}