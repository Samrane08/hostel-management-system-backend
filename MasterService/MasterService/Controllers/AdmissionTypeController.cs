using MasterService.Models;
using MasterService.Service.Implemetation;
using MasterService.Service.Interface;
using MasterService.Service.Utility;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace MasterService.Controllers;

public class AdmissionTypeController : APIBaseController
{
    private readonly IDapper dapper;
    private readonly ICacheService cacheService;
    private readonly IErrorLogger errorLogger;


    public AdmissionTypeController(IDapper dapper, ICacheService cacheService, IErrorLogger errorLogger)
    {
        this.dapper = dapper;
        this.cacheService = cacheService;
        this.errorLogger = errorLogger;

       
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        try
        {
           
            var cachedSpecialties = await cacheService.GetOrCreateAsync("AdmissionType", async () => { return await AdmissionType(); }, TimeSpan.FromDays(30));
            return Ok(cachedSpecialties);
        }
        catch (Exception ex)
        {
            // ExceptionLogging.LogException(Convert.ToString(ex));

            return Ok(new List<string>());
        }
    }

    private async Task<List<SelectListModel>> AdmissionType()
    {
        try
        {
            var result = await Task.FromResult(dapper.GetAll<SelectListModel>("usp_GetAdmissionType", null, commandType: CommandType.StoredProcedure));
            return result;
        }
        catch (Exception ex)
        {
            // ExceptionLogging.LogException(Convert.ToString(ex));
            await errorLogger.Log("usp_GetAdmissionType", ex);

            return new List<SelectListModel>();
        }
    }
}
