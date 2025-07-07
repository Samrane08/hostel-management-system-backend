using Dapper;
using MasterService.Models;
using MasterService.Service.Interface;
using MasterService.Service.Utility;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace MasterService.Controllers;

public class YearsOfStudyController : APIBaseController
{
    private readonly IDapper dapper;

    public YearsOfStudyController(IDapper dapper)
    {
        this.dapper = dapper;
    }

    [HttpGet]
    public async Task<IActionResult> Get(int CourseId, int? LangId=1)
    {
        try
        {
            var myparam = new DynamicParameters();
            myparam.Add("_intID", CourseId, DbType.Int32);
            myparam.Add("_LangId", LangId, DbType.Int32);

            var result = await Task.FromResult(dapper.GetAll<SelectListModel>("usp_GetCurrentQualificationYear", myparam, commandType:System.Data.CommandType.StoredProcedure));
            return Ok(result);
        }
        catch (Exception ex)
        {
            // ExceptionLogging.LogException(Convert.ToString(ex));
            return Ok(new List<string>());
        }
    }
}
