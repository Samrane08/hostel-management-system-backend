using Dapper;
using MasterService.Models;
using MasterService.Service.Interface;
using MasterService.Service.Utility;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace MasterService.Controllers;

public class CourseTypeController : APIBaseController
{
    private readonly IDapper dapper;
    private readonly ICurrentUserService currentUserService;
    private readonly IErrorLogger errorLogger;

    public CourseTypeController(IDapper dapper, ICurrentUserService currentUserService, IErrorLogger errorLogger)
    {
        this.dapper = dapper;
        this.currentUserService = currentUserService;
        this.errorLogger = errorLogger;
    }

    [HttpGet]
    public async Task<IActionResult> Get(int? deptId , string? IsSwadharorHostel)
    {
        try
        {
            var myparam = new DynamicParameters();
            myparam.Add("p_deptId", (deptId == null || deptId == 0) ? Convert.ToInt32(currentUserService.DeptId) : deptId, DbType.Int32);

            if (IsSwadharorHostel != null)
                myparam.Add("p_IsSwadharorHostel", Convert.ToInt32(IsSwadharorHostel), DbType.Int16);
            else
                myparam.Add("p_IsSwadharorHostel", 0 , DbType.Int16);
            var result = await Task.FromResult(dapper.GetAll<SelectListModel>("usp_GetCourseType", myparam, commandType: System.Data.CommandType.StoredProcedure));
            return Ok(result);
        }
        catch (Exception ex)
        {
            // ExceptionLogging.LogException(Convert.ToString(ex));
            await errorLogger.Log("usp_GetCourseType_Swapnil", ex);
            return Ok(new List<string>());
        }
    }

    [HttpGet("get-all-coursetype")]
    public async Task<IActionResult> GetAllCourseType()
    {
        try
        {
            var myparam = new DynamicParameters();
            myparam.Add("p_deptId",  Convert.ToInt32(currentUserService.DeptId) , DbType.Int32);

            var result = await Task.FromResult(dapper.GetAll<SelectListModel>("usp_GetAllCourseType", myparam, commandType: System.Data.CommandType.StoredProcedure));
            return Ok(result);
        }
        catch (Exception ex)
        {
            // ExceptionLogging.LogException(Convert.ToString(ex));
            await errorLogger.Log("usp_GetAllCourseType", ex);
            return Ok(new List<string>());
        }
    }



}
