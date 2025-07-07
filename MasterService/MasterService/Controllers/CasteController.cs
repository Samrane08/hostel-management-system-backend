using Dapper;
using MasterService.Models;
using MasterService.Service.Implemetation;
using MasterService.Service.Interface;
using MasterService.Service.Utility;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace MasterService.Controllers;

public class CasteController : APIBaseController
{
    private readonly IDapper dapper;
    private readonly ICurrentUserService currentUserService;
    private readonly IErrorLogger errorLogger;

    public CasteController(IDapper dapper, ICurrentUserService currentUserService, IErrorLogger errorLogger)
    {
        this.dapper = dapper;
        this.currentUserService = currentUserService;
        this.errorLogger = errorLogger;
    }

    [HttpGet]
    public async Task<IActionResult> Get(int castecategoryId)
    {
        try
        {
            var myparam = new DynamicParameters();
            myparam.Add("@castecategoryId", castecategoryId, DbType.String);
         
            var result = await Task.FromResult(dapper.GetAll<SelectListModel>("usp_GetCasteByCategoryId", myparam, commandType:System.Data.CommandType.StoredProcedure));
            return Ok(result);
        }
        catch (Exception ex)
        {
            // ExceptionLogging.LogException(Convert.ToString(ex));
            await errorLogger.Log("usp_GetAllAvailableAcademicyear", ex);
            return Ok(new List<string>());
        }
    }

    [HttpGet("CasteBasedOnServiceType")]
    public async Task<IActionResult> GetCategoryBasedOnServiceType(int castecategoryId, int deptId, int ServiceType)
    {
        try
        {
            if (deptId == 0)
                deptId = Convert.ToInt32(currentUserService.DeptId);

            var myparam = new DynamicParameters();
            myparam.Add("p_deptId", deptId, DbType.Int32);
            myparam.Add("p_serviceType", ServiceType, DbType.Int32);
            myparam.Add("@castecategoryId", castecategoryId, DbType.String);
            var result = await Task.FromResult(dapper.GetAll<SelectListModel>("usp_GetCasteByCategoryIdServiceType", myparam, commandType: System.Data.CommandType.StoredProcedure));
            return Ok(result);
        }
        catch (Exception ex)
        {
            await errorLogger.Log("usp_GetCasteByCategoryIdServiceType", ex);
            return Ok(new List<string>());
        }
    }
}
