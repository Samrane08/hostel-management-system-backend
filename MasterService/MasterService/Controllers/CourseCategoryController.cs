using Dapper;
using MasterService.Models;
using MasterService.Service.Interface;
using MasterService.Service.Utility;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Data;

namespace MasterService.Controllers;

public class CourseCategoryController : APIBaseController
{
    private readonly IDapper dapper;
    private readonly IErrorLogger errorLogger;

    public CourseCategoryController(IDapper dapper, IErrorLogger errorLogger)
    {
        this.dapper = dapper;
        this.errorLogger = errorLogger;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        try
        {

            var result = await Task.FromResult(dapper.GetAll<SelectListModel>("usp_GetCourseCategory", null, commandType:System.Data.CommandType.StoredProcedure));
            return Ok(result);
        }
        catch (Exception ex)
        {
            // ExceptionLogging.LogException(Convert.ToString(ex));
            await errorLogger.Log("usp_GetCourseCategory", ex);
            return Ok(new List<string>());
        }
    }
}
