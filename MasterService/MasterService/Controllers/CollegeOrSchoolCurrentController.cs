using Dapper;
using MasterService.Models;
using MasterService.Service.Interface;
using MasterService.Service.Utility;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace MasterService.Controllers;

public class CollegeOrSchoolCurrentController : APIBaseController
{
    private readonly IDapper dapper;
    private readonly IErrorLogger errorLogger;

    public CollegeOrSchoolCurrentController(IDapper dapper, IErrorLogger errorLogger)
    {
        this.dapper = dapper;
        this.errorLogger = errorLogger;
    }

    [HttpGet]
    public async Task<IActionResult> Get(int stream,int qualificationType, int districtid ,int? LangId)
    {
        try
        {
            List< SelectListModel> result=null;
          
            if (qualificationType == 14)
            {
                var myparam = new DynamicParameters();
                myparam.Add("p_district_code_census", districtid, DbType.Int64);
                result = await Task.FromResult(dapper.GetAll<SelectListModel>("usp_FilterSchoolsForPostPast", myparam, commandType: System.Data.CommandType.StoredProcedure));

            }
            else
            {
                var myparam = new DynamicParameters();
                myparam.Add("@intID", stream, DbType.Int32);
                myparam.Add("@intID_extra", qualificationType, DbType.Int32);
                myparam.Add("@intID_extra1", districtid, DbType.Int32);
                myparam.Add("@LangId", LangId, DbType.Int32);
                result = await Task.FromResult(dapper.GetAll<SelectListModel>("USP_GetCollegeOrSchoolList_Current", myparam, commandType: System.Data.CommandType.StoredProcedure));
            }
            return Ok(result);
        }
        catch (Exception ex)
        {
            // ExceptionLogging.LogException(Convert.ToString(ex));
            await errorLogger.Log("USP_GetCollegeOrSchoolList_Current / usp_FilterSchoolsForPostPast", ex);


            return Ok(new List<string>());
        }
    }
}
