using MasterService.Models;
using MasterService.Models.ReqModel;
using MasterService.Service.Interface;
using MasterService.Service.Utility;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace MasterService.Controllers;
public class DistrictController : APIBaseController
{
    private readonly IDapper dapper;
    private readonly ICacheService cacheService;
    private readonly IErrorLogger errorLogger;

    public DistrictController(IDapper dapper, ICacheService cacheService)
    {
        this.dapper = dapper;
        this.cacheService = cacheService;
    }

    [HttpGet]
    public async Task<IActionResult> Get(int? state, int? division, int? lang = 1)
    {
        try
        {
            var response = new List<SelectListModel>();
            try
            {
                var result = await cacheService.GetOrCreateAsync("DistrictList", async () => { return await DistList(); }, TimeSpan.FromDays(30));

                if (result != null && result.Count > 0)
                    result = result.Where(x => x.Lang == lang).ToList();

                if (result != null && result.Count > 0 && state.HasValue)
                    result = result.Where(x => x.StateId == state).ToList();


            if (result != null && result.Count > 0 && division.HasValue)
                result = result.Where(x => x.DivisionId == division).ToList();

                if (result != null && result.Count > 0)
                    response = result.Select(x => new SelectListModel { Value = x.Id.ToString(), Text = x.Name }).ToList();

                if (result != null && result.Count > 0)
                    response = result.Select(x => new SelectListModel { Value = x.Id.ToString(), Text = x.Name }).ToList();
            }
            catch (Exception ex)
            {
                // ExceptionLogging.LogException(Convert.ToString(ex));
            }
            if (response == null || response.Count == 0)
            {
                var Data = await DistList();

                if (Data != null && Data.Count > 0)
                    Data = Data.Where(x => x.Lang == lang).ToList();

                if (Data != null && Data.Count > 0 && state.HasValue)
                    Data = Data.Where(x => x.StateId == state).ToList();

                if (Data != null && Data.Count > 0 && division.HasValue)
                    Data = Data.Where(x => x.DivisionId == division).ToList();

                if (Data != null && Data.Count > 0)
                    response = Data.Select(x => new SelectListModel { Value = x.Id.ToString(), Text = x.Name }).ToList();
            }
            return Ok(response);
        }
        catch (Exception ex)
        {
            // ExceptionLogging.LogException(Convert.ToString(ex));
            return Ok(new List<string>());
        }
    }
    private async Task<List<DistrictModel>> DistList()
    {
        try
        {   //usp_GetDistrictList
            var result = await Task.FromResult(dapper.GetAll<DistrictModel>("usp_DistrictMaster", null, commandType: System.Data.CommandType.StoredProcedure));
            return result;
        }
        catch (Exception ex)
        {
            // ExceptionLogging.LogException(Convert.ToString(ex));
            await errorLogger.Log("usp_DistrictMaster", ex);
            return new List<DistrictModel>();
        }
    }
}
