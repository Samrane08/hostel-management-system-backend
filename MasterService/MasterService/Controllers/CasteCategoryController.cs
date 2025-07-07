using Dapper;
using MasterService.Models;
using MasterService.Service.Implemetation;
using MasterService.Service.Interface;
using MasterService.Service.Utility;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace MasterService.Controllers;

public class CasteCategoryController : APIBaseController
{
    private readonly IDapper dapper;
    private readonly ICurrentUserService currentUserService;
    private readonly IErrorLogger errorLogger;


    public CasteCategoryController(IDapper dapper, ICurrentUserService currentUserService, IErrorLogger errorLogger)
    {
        this.dapper = dapper;
        this.currentUserService = currentUserService;
        this.errorLogger = errorLogger;
    }

    [HttpGet]
    public async Task<IActionResult> Get(bool? IsSwadhar, int deptId)
    {
        try
        {
            if(deptId == 0)
                deptId=Convert.ToInt32(currentUserService.DeptId);

            var myparam = new DynamicParameters();
            myparam.Add("p_deptId", deptId);
            var result = await Task.FromResult(dapper.GetAll<SelectListModel>("usp_GetCasteCategory", myparam, commandType: System.Data.CommandType.StoredProcedure));

            if (IsSwadhar == true)
            {
                result = result.Where(x=>x.Value == "2").ToList();
            }
            return Ok(result);
        }
        catch (Exception ex)
        {
            await errorLogger.Log("usp_GetCasteCategory", ex);

            return Ok(new List<string>());
        }
    }

    [HttpGet("CasteCategoryBasedOnServiceType")]
    public async Task<IActionResult> GetCategoryBasedOnServiceType(bool? IsSwadhar, int deptId, int ServiceType)
    {
        try
        {
            if (deptId == 0)
                deptId = Convert.ToInt32(currentUserService.DeptId);

            var myparam = new DynamicParameters();
            myparam.Add("p_deptId", deptId, DbType.Int32);
            myparam.Add("p_serviceType", ServiceType, DbType.Int32);
            var result = await Task.FromResult(dapper.GetAll<SelectListModel>("usp_GetCasteCategoryServiceType", myparam, commandType: System.Data.CommandType.StoredProcedure));

            if (IsSwadhar == true)
            {
                result = result.Where(x => x.Value == "2").ToList();
            }
            return Ok(result);
        }
        catch (Exception ex)
        {
            await errorLogger.Log("usp_GetCasteCategoryServiceType", ex);
            return Ok(new List<string>());
        }
    }
}
