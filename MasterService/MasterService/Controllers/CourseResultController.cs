using Dapper;
using MasterService.Models;
using MasterService.Service.Interface;
using MasterService.Service.Utility;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace MasterService.Controllers;

public class CourseResultController : APIBaseController
{
    private readonly IDapper dapper;
    private readonly IErrorLogger errorLogger;

    public CourseResultController(IDapper dapper, IErrorLogger errorLogger)
    {
        this.dapper = dapper;
        this.errorLogger = errorLogger;
    }

    [HttpGet]
    public async Task<IActionResult> Get(string caller)
    {
        try
        {

            var myparam = new DynamicParameters();
            myparam.Add("caller", caller, DbType.String);
            var result = await Task.FromResult(dapper.GetAll<SelectListModel>("usp_GetCourseResult", myparam, commandType:System.Data.CommandType.StoredProcedure));
            return Ok(result);
        }
        catch (Exception ex)
        {
            await errorLogger.Log("usp_GetCourseResult", ex);
            // ExceptionLogging.LogException(Convert.ToString(ex));
            return Ok(new List<string>());
        }
    }
}
