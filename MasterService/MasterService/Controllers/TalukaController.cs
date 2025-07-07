using MasterService.Models;
using MasterService.Models.ReqModel;
using MasterService.Service.Interface;
using MasterService.Service.Utility;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace MasterService.Controllers;
public class TalukaController : APIBaseController
{
    private readonly IDapper dapper;
    private readonly ICacheService cacheService;

    public TalukaController(IDapper dapper, ICacheService cacheService)
    {
        this.dapper = dapper;
        this.cacheService = cacheService;
    }
    [HttpGet]
    public async Task<IActionResult> Get(int? distid,int? lang = 1)
    {
        try
        {
            var response = new List<SelectListModel>();
            try
            {
                var result = await cacheService.GetOrCreateAsync("TalukaList", async () => { return await TalukaList(); }, TimeSpan.FromDays(30));

                if (result != null && result.Count > 0 && distid.HasValue)
                    result = result.Where(x => x.DistrictId == distid).ToList();

                if (result != null && result.Count > 0)
                    result = result.Where(x => x.Lang == lang).ToList();

                if (result != null && result.Count > 0)
                    response = result.Select(x => new SelectListModel { Value = x.Id.ToString(), Text = x.Name })
                                     .ToList();
            }
            catch (Exception)
            {

                
            }            

            if(response.Count == 0)
            {
                var Data = await TalukaList();

                if (Data != null && Data.Count > 0 && distid.HasValue)
                    Data = Data.Where(x => x.DistrictId == distid).ToList();

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
            // ExceptionLogging.LogException(Convert.ToString(ex));
            return Ok(new List<string>());
        }
    }

    private async Task<List<TalukaModel>> TalukaList()
    {
        try
        {   //usp_GetTalukaList
            var result = await Task.FromResult(dapper.GetAll<TalukaModel>("usp_TalukaMaster", null, commandType: System.Data.CommandType.StoredProcedure));
            return result;
        }
        catch (Exception ex)
        {
            // ExceptionLogging.LogException(Convert.ToString(ex));
            return new List<TalukaModel>();
        }
    }
}
