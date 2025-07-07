using MasterService.Models;
using MasterService.Models.ReqModel;
using MasterService.Service.Interface;
using MasterService.Service.Utility;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace MasterService.Controllers;
public class MultipleTalukaController : APIBaseController
{
    private readonly IDapper dapper;
    private readonly ICacheService cacheService;
    private readonly IErrorLogger errorLogger;

    public MultipleTalukaController(IDapper dapper, ICacheService cacheService, IErrorLogger errorLogger)
    {
        this.dapper = dapper;
        this.cacheService = cacheService;
        this.errorLogger = errorLogger;
    }

    [HttpPost]
    public async Task<IActionResult> Get([FromBody] List<int> distid)
    {
        try
        {
            var response = new List<SelectListModel>();
            var result = await cacheService.GetOrCreateAsync("TalukaList", async () => { return await TalukaList(); }, TimeSpan.FromDays(30));

            if (result != null && result.Count > 0)
                result = result.Where(x => distid.Contains(x.DistrictId)).ToList();

            if (result != null && result.Count > 0)
                result = result.Where(x => x.Lang == 1).ToList();

            if (result != null && result.Count > 0)
                response = result.Select(x => new SelectListModel { Value = x.Id.ToString(), Text = x.Name })
                                 .ToList();
            if (response.Count == 0)
            {
                var Data = await TalukaList();

                if (Data != null && Data.Count > 0)
                    Data = Data.Where(x => distid.Contains(x.DistrictId)).ToList();

                if (Data != null && Data.Count > 0)
                    Data = Data.Where(x => x.Lang == 1).ToList();

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
        {  
            var result = await Task.FromResult(dapper.GetAll<TalukaModel>("usp_TalukaMaster", null, commandType: System.Data.CommandType.StoredProcedure));
            return result;
        }
        catch (Exception ex)
        {
            // ExceptionLogging.LogException(Convert.ToString(ex));
            await errorLogger.Log("usp_TalukaMaster", ex);

            return new List<TalukaModel>();
        }
    }
}
