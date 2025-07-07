using Dapper;
using MasterService.Models;
using MasterService.Service.Interface;
using MasterService.Service.Utility;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace MasterService.Controllers;

public class StateController : APIBaseController
{
    private readonly IDapper dapper;
    private readonly ICacheService cacheService;

    public StateController(IDapper dapper, ICacheService cacheService)
    {
        this.dapper = dapper;
        this.cacheService = cacheService;
    }

    [HttpGet]
    public async Task<IActionResult> Get(int? lang)
    {
        try
        {
            if (lang.HasValue)
            {
                var cachedSpecialties = await cacheService.GetOrCreateAsync("StateListWith" + lang.ToString(), async () => { return await StateList(lang);},TimeSpan.FromDays(30));
                if(cachedSpecialties == null || cachedSpecialties.Count == 0)
                {
                    return Ok(await StateList(lang));
                }
                return Ok(cachedSpecialties);
            }
            else
            {
                var cachedSpecialties = await cacheService.GetOrCreateAsync("StateList", async () => { return await StateList(lang); }, TimeSpan.FromDays(30));
                if (cachedSpecialties == null || cachedSpecialties.Count == 0)
                {
                    return Ok(await StateList(lang));
                }
                return Ok(cachedSpecialties);
            }
        }
        catch (Exception ex)
        {
            // ExceptionLogging.LogException(Convert.ToString(ex));
            return Ok(new List<string>());
        }
    }
    private async Task<List<SelectListModel>> StateList(int? lang)
    {
        try
        {
            var myparam = new DynamicParameters();
            myparam.Add("_Lang", lang, DbType.Int32);
            var result = await Task.FromResult(dapper.GetAll<SelectListModel>("usp_GetStateList", myparam, commandType: System.Data.CommandType.StoredProcedure));
            return result;
        }
        catch (Exception ex)
        {
            // ExceptionLogging.LogException(Convert.ToString(ex));
            return new List<SelectListModel>();
        }
    }
}
