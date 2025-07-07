using Dapper;
using MasterService.Models;
using MasterService.Service.Interface;
using MasterService.Service.Utility;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Data;

namespace MasterService.Controllers;

public class CourseYearStatusController : APIBaseController
{
    private readonly IDapper dapper;
    private readonly IErrorLogger errorLogger;

    public CourseYearStatusController(IDapper dapper, IErrorLogger errorLogger)
    {
        this.dapper = dapper;
        this.errorLogger = errorLogger;
    }

    [HttpGet]
    public async Task<IActionResult> Get(int yearId)
    {
        try
        {
            var myparam = new DynamicParameters();
            myparam.Add("_YearId", yearId, DbType.Int32);
           
         

            var result = await Task.FromResult(dapper.GetAll<SelectListModel>("usp_GetCourseStatusYear", myparam, commandType:System.Data.CommandType.StoredProcedure));
            return Ok(result);
        }
        catch (Exception ex)
        {
            // ExceptionLogging.LogException(Convert.ToString(ex));
            await errorLogger.Log("usp_GetCourseStatusYear", ex);
            return Ok(new List<string>());
        }
    }
}
