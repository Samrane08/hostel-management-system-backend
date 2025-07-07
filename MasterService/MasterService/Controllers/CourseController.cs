using Dapper;
using MasterService.Models;
using MasterService.Service.Interface;
using MasterService.Service.Utility;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace MasterService.Controllers;

public class CourseController : APIBaseController
{
    private readonly IDapper dapper;
    private readonly IErrorLogger errorLogger;

    public CourseController(IDapper dapper, IErrorLogger errorLogger)
    {
        this.dapper = dapper;
        this.errorLogger = errorLogger;
    }

    [HttpGet]
    public async Task<IActionResult> Get(int cold,int? strmId,int? quaId,int? langId=1)
    {
        try
        {
            var myparam = new DynamicParameters();
            myparam.Add("p_CollegeID", cold, DbType.Int32);
            myparam.Add("p_StreamID", strmId, DbType.Int32);
            myparam.Add("p_QualificationID", quaId, DbType.Int32);
            myparam.Add("p_LangId", langId, DbType.Int32);
            
            var result = await Task.FromResult(dapper.GetAll<SelectListModel>("usp_GetCourseName", myparam, commandType:System.Data.CommandType.StoredProcedure));
            return Ok(result);
        }
        catch (Exception ex)
        {
            // ExceptionLogging.LogException(Convert.ToString(ex));
            await errorLogger.Log("usp_GetCourseName", ex);
            return Ok(new List<string>());
        }
    }
}
